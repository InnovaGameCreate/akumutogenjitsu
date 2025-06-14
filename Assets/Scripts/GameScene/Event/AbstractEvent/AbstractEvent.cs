using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public abstract class AbstractEvent : MonoBehaviour
{
    [Header("�C�x���g�̐ݒ�")]
    // �C�x���g�̎��
    [SerializeField] private eEvent _event;

    [Header("1�񂾂����s����")]
    // i�񂾂��g���K�[���邩
    [SerializeField] private bool _isTriggeredOnce;

    // �C�x���g�̏��
    private eEventStatus _eventStatus = eEventStatus.NotTriggered;

    [Header("�C�x���gID�i���������E�ύX���Ȃ��ł��������j")]
    [SerializeField] private string _eventId;

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

    void Start()
    {
        OnStartEvent();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdateEvent();

        if (_eventStatus == eEventStatus.Triggered)
        {
            BasicAnimation _basicAnimation = GetComponent<BasicAnimation>();
            if (_basicAnimation == null)
            {
                Debug.LogError("BasicAnimation���A�^�b�`����Ă��܂���");
            }
            _basicAnimation.Enabled = false; // 1�񂾂����s����ꍇ�̓A�j���[�V�����𖳌��ɂ���
            return;
        }

        // �C�x���g�����s���̓g���K�[���Ȃ�
        if (_eventStatus != eEventStatus.Running)
        {
            if (IsTriggerEvent())
            {
                SetIsUnitMove(false); // Unit�̈ړ��𖳌��ɂ���

                TriggerEvent();
                _eventStatus = eEventStatus.Running;
            }
        }

        if (IsFinishEvent() && _eventStatus == eEventStatus.Running)
        {
            SetIsUnitMove(true); // Unit�̈ړ���L���ɂ���

            if (_isTriggeredOnce)
            {
                _eventStatus = eEventStatus.Triggered;
            }
            else
            {
                _eventStatus = eEventStatus.NotTriggered;
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
            return _eventStatus;
        }
        set
        {
            _eventStatus = value;
        }
    }
}
