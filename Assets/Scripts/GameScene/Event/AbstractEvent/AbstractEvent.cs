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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_isTriggeredOnce && _eventStatus == eEventStatus.Triggered)
        {
            return;
        }

        // �C�x���g�����s���̓g���K�[���Ȃ�
        if (_eventStatus != eEventStatus.Running)
        {
            if (IsTriggerEvent())
            {
                TriggerEvent();
                _eventStatus = eEventStatus.Running;
            }
        }

        if (IsFinishEvent())
        {
            _eventStatus = eEventStatus.Triggered;

#if DEBUG_MODE
            Debug.Log($"�C�x���g: {_event} ���I�����܂���");
#endif
        }

        OnUpdateEvent();
    }

    /// <summary>
    /// �C�x���g�̍X�V����(Update()�̑���)
    /// </summary>
    public abstract void OnUpdateEvent();

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
    protected eEventStatus EventStatus
    {
        get { return _eventStatus; }
    }
}
