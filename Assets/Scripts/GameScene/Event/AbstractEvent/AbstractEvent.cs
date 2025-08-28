using System.Collections.Generic;
using R3;
using UnityEngine;

public abstract class AbstractEvent : MonoBehaviour
{
    [Header("�C�x���g�̐ݒ�")]
    // �C�x���g�̎��
    [SerializeField] private eEvent _event;

    [Header("1�񂾂����s����")]
    // i�񂾂��g���K�[���邩
    [SerializeField] private bool _isTriggeredOnce;

    [Header("�C�x���gID�i���������E�ύX���Ȃ��ł��������j")]
    [SerializeField] private string _eventId;

    [Header("StoryLayer(0�̂Ƃ��͏�ɗL���ŁA�w�肷��Ƃ���1�ȏ�ɐݒ肷��B)")]
    [SerializeField] private int _storyLayer = 0;

    [Header("���̃C�x���g���I��������StoryLayer���グ�邩")]
    [SerializeField] private bool _isUpStoryLayer = false;

    private StoryManager _storyManager;
    private EventManager _eventManager;

    // �ۑ�����f�[�^
    private EventData _eventData = new EventData();

    // ���������ꂽ��
    private readonly ReactiveProperty<bool> _initialized = new(false);
    public ReadOnlyReactiveProperty<bool> Initialized => _initialized;

    /// <summary>
    /// �C�x���gID
    /// </summary>
    public string EventId
    {
        get
        {
            return _eventId;
        }
        set
        {
            _eventId = value;
            _eventData.EventId = value;
        }
    }

    void Start()
    {
        _storyManager = GameObject.FindGameObjectWithTag("StoryMgr").GetComponent<StoryManager>();
        if (_storyManager == null)
        {
            Debug.LogError("StoryManager�����݂��܂���B");
            return;
        }
        _eventManager = FindAnyObjectByType<EventManager>();
        if (_eventManager == null)
        {
            Debug.LogError("EventManager�����݂��܂���B");
            return;
        }

        if (_eventManager.HasLoadedEvent(_eventId))
        {
            _eventData = _eventManager.LoadEventData(_eventId);
        }
        else
        {
            // EventData�̏�����
            _eventData.EventType = _event;
            _eventData.EventId = _eventId;
            _eventData.EventStatus = eEventStatus.NotTriggered;
            _eventData.Enabled = true;
            _eventManager.SaveEventData(_eventId, _eventData);
        }

        OnStartEvent();

        _initialized.Value = true;
    }

    // Update is called once per frame
    void Update()
    {
        // EventStory��EventStatus�ŗL��/���������߂�
        if (_storyManager.CurrentStoryLayer == _storyLayer || _storyLayer == 0)
        {
            _eventData.Enabled = _eventData.EventStatus != eEventStatus.Triggered;
        }
        else
        {
            _eventData.Enabled = false;
        }

        // �����̏ꍇ�\���������ď������~�߂�
        if (_eventData.Enabled == false)
        {
            BasicAnimation animation = gameObject.GetComponent<BasicAnimation>();
            if (animation != null)
            {
                // EventQueue���g�����Ƃ�SetActive(false);������Anull�ɂȂ�\��������B
                animation.Enabled = false;
            }
            _eventManager.SaveEventData(_eventId, _eventData);
            return;
        }

        OnUpdateEvent();

        // �C�x���g�����s���̓g���K�[���Ȃ�
        if (_eventData.EventStatus != eEventStatus.Running)
        {
            if (IsTriggerEvent())
            {
                SetIsUnitMove(false); // Unit�̈ړ��𖳌��ɂ���

                TriggerEvent();
                _eventData.EventStatus = eEventStatus.Running;
            }
        }

        if (IsFinishEvent() && _eventData.EventStatus == eEventStatus.Running)
        {
            SetIsUnitMove(true); // Unit�̈ړ���L���ɂ���

            if (_isTriggeredOnce)
            {
                _eventData.EventStatus = eEventStatus.Triggered;
            }
            else
            {
                _eventData.EventStatus = eEventStatus.NotTriggered;
            }
            if (_isUpStoryLayer)
            {
                _storyManager.CurrentStoryLayer++;
            }

#if DEBUG_MODE
            Debug.Log($"�C�x���g: {_event} ���I�����܂���");
#endif
        }
    }

    /// <summary>
    /// Unit�̈ړ���L��/�����ɂ���
    /// </summary>
    /// <param name="isUnitMove"> �L��/���� </param>
    private void SetIsUnitMove(bool isUnitMove)
    {
        // �S�Ă�Unit��UnitMove���擾
        List<UnitMove> units = new List<UnitMove>(FindObjectsByType<UnitMove>(FindObjectsSortMode.None));

        foreach (UnitMove unit in units)
        {
            unit.IsEnabled = isUnitMove;
        }
    }

    /// <summary>
    /// EventData��K������
    /// </summary>
    /// <param name="eventData"> EventManager�ɂ���EventData </param>
    public void InitWithEventData(EventData eventData)
    {
        _eventData = eventData;
        Debug.Log($"type: {_eventData.EventType}, enabled: {_eventData.Enabled}");
    }

    /// <summary>
    /// �C�x���g�̏���������(Start()�̑���)
    /// </summary>
    public virtual void OnStartEvent()
    {
    }

    /// <summary>
    /// �C�x���g�̍X�V����(Update()�̑���)
    /// </summary>
    public virtual void OnUpdateEvent()
    {
    }

    /// <summary>
    /// �C�x���g�̃g���K�[�̏���
    /// </summary>
    /// <returns> �C�x���g���g���K�[���邩 </returns>
    public abstract bool IsTriggerEvent();

    /// <summary>
    /// �C�x���g���g���K�[����
    /// </summary>
    public abstract void TriggerEvent();

    /// <summary>
    /// �C�x���g���I��������
    /// </summary>
    /// <returns> �I�������� </returns>
    public abstract bool IsFinishEvent();

    /// <summary>
    /// �C�x���g�̎��(ReadOnly)
    /// </summary>
    protected eEvent Event
    {
        get { return _event; }
    }

    /// <summary>
    /// �C�x���g�̏��(ReadOnly)
    /// </summary>
    public eEventStatus EventStatus
    {
        get
        {
            return _eventData.EventStatus;
        }
        set
        {
            _eventData.EventStatus = value;
        }
    }

    /// <summary>
    /// �ۑ�����C�x���g�̃f�[�^
    /// </summary>
    public EventData EventData
    {
        get
        {
            return _eventData;
        }
        set
        {
            _eventData = value;
        }
    }

    /// <summary>
    /// 1�x�������s���Ȃ���
    /// </summary>
    public bool TriggerOnce
    {
        get => _isTriggeredOnce;
        set
        {
            _isTriggeredOnce = value;
        }
    }

    /// <summary>
    /// �L����
    /// </summary>
    public bool Enabled
    {
        get => _eventData.Enabled;
        set => _eventData.Enabled = value;
    }
}
