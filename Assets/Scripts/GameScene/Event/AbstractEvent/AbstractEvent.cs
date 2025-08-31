using System;
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
        }
    }

    /// <summary>
    /// �����I�ɃC�x���g�����s����
    /// </summary>
    public void TriggerEventForce()
    {
        EventStatus = eEventStatus.Running;
        TriggerEvent();
    }

    void Start()
    {
        if (EventManager.Instance == null)
        {
            Debug.LogError("EventManager�����݂��܂���B");
            return;
        }

        OnStartEvent();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdateEvent();

        // �C�x���g�����s���̓g���K�[���Ȃ�
        if (EventManager.Instance.GetEventData(_eventId).EventStatus != eEventStatus.Running)
        {
            if (IsTriggerEvent())
            {
                SetIsUnitMove(false); // Unit�̈ړ��𖳌��ɂ���

                TriggerEvent();

                EventManager.Instance.SetEventStatus(_eventId, eEventStatus.Running);
            }
        }

        if (IsFinishEvent() && EventManager.Instance.GetEventData(_eventId).EventStatus == eEventStatus.Running)
        {
            SetIsUnitMove(true); // Unit�̈ړ���L���ɂ���

            if (_isTriggeredOnce)
            {
                EventManager.Instance.SetEventStatus(_eventId, eEventStatus.Triggered);
            }
            else
            {
                EventManager.Instance.SetEventStatus(_eventId, eEventStatus.NotTriggered);
            }
            if (_isUpStoryLayer)
            {
                StoryManager.Instance.CurrentStoryLayer++;
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
            return EventManager.Instance.GetEventData(_eventId).EventStatus;
        }
        set
        {
            EventManager.Instance.SetEventStatus(_eventId, value);
        }
    }

    /// <summary>
    /// �ۑ�����C�x���g�̃f�[�^
    /// </summary>
    public EventData EventData
    {
        get
        {
            return EventManager.Instance.GetEventData(_eventId);
        }
        set
        {
            EventManager.Instance.SetEventData(_eventId, value);
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
        get => EventManager.Instance.GetEventData(_eventId).Enabled;
        set => EventManager.Instance.SetEventEnabled(_eventId, value);
    }

    /// <summary>
    /// StoryLayer
    /// </summary>
    public int StoryLayer
    {
        get => _storyLayer;
    }

    /// <summary>
    /// �f�t�H���g��EventData
    /// </summary>
    public EventData DefaultEventData
    {
        get
        {
            EventData eventData = new EventData();
            eventData.EventId = _eventId;
            eventData.EventStatus = eEventStatus.NotTriggered;
            eventData.EventType = _event;
            eventData.Enabled = true;

            return eventData;
        }
    }
}
