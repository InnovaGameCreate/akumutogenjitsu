using UnityEngine;

public abstract class AbstractEvent : MonoBehaviour
{
    // �C�x���g�̎��
    [SerializeField] private eEvent _event;

    // i�񂾂��g���K�[���邩
    [SerializeField] private bool _isTriggeredOnce;

    // �g���K�[���ꂽ��
    private bool _hasTriggeredOnce;

    // �C�x���g�����s����
    private bool _isEventRunning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_isTriggeredOnce && _hasTriggeredOnce)
        {
            return;
        }

        // �C�x���g�����s���̓g���K�[���Ȃ�
        if (!_isEventRunning)
        {
            if (IsTriggerEvent())
            {
                TriggerEvent();
                _hasTriggeredOnce = true;
                _isEventRunning = true;
            }
        }
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
    /// �C�x���g�̎��(ReadOnly)
    /// </summary>
    protected eEvent Event
    {
        get { return _event; }
    }

    /// <summary>
    /// �C�x���g��1��g���K�[���ꂽ��(ReadOnly)
    /// </summary>
    protected bool HasTriggeredOnce
    {
        get { return _hasTriggeredOnce; }
    }

    /// <summary>
    /// �C�x���g�����s����(ReadOnly)
    /// </summary>
    protected bool IsRunningEvent
    {
        get { return _isEventRunning; }
    }
}
