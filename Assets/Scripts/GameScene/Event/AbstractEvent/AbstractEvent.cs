using UnityEngine;

public abstract class AbstractEvent : MonoBehaviour
{
    // イベントの種類
    [SerializeField] private eEvent _event;

    // i回だけトリガーするか
    [SerializeField] private bool _isTriggeredOnce;

    // トリガーされたか
    private bool _hasTriggeredOnce;

    // イベントが実行中か
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

        // イベントが実行中はトリガーしない
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
    /// イベントのトリガーの条件
    /// </summary>
    /// <returns> イベントをトリガーするか </returns>
    public abstract bool IsTriggerEvent();

    /// <summary>
    /// イベントをトリガーする
    /// </summary>
    public abstract void TriggerEvent();

    /// <summary>
    /// イベントの種類(ReadOnly)
    /// </summary>
    protected eEvent Event
    {
        get { return _event; }
    }

    /// <summary>
    /// イベントが1回トリガーされたか(ReadOnly)
    /// </summary>
    protected bool HasTriggeredOnce
    {
        get { return _hasTriggeredOnce; }
    }

    /// <summary>
    /// イベントが実行中か(ReadOnly)
    /// </summary>
    protected bool IsRunningEvent
    {
        get { return _isEventRunning; }
    }
}
