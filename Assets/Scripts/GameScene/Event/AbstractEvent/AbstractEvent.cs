using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

public abstract class AbstractEvent : MonoBehaviour
{
    [Header("イベントの設定")]
    // イベントの種類
    [SerializeField] private eEvent _event;

    [Header("1回だけ実行する")]
    // i回だけトリガーするか
    [SerializeField] private bool _isTriggeredOnce;

    [Header("イベントID（自動生成・変更しないでください）")]
    [SerializeField] private string _eventId;

    [Header("StoryLayer(0のときは常に有効で、指定するときは1以上に設定する。)")]
    [SerializeField] private int _storyLayer = 0;

    [Header("このイベントが終了したらStoryLayerを上げるか")]
    [SerializeField] private bool _isUpStoryLayer = false;

    protected readonly Subject<Unit> onTriggerEvent = new();
    protected readonly Subject<Unit> onFinishEvent = new();

    private CompositeDisposable _disposable = new();

    /// <summary>
    /// イベントID
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
    /// 強制的にイベントを実行する
    /// </summary>
    public void TriggerEventForce()
    {
        onTriggerEvent.OnNext(Unit.Default);
    }

    void Start()
    {
        if (EventManager.Instance == null)
        {
            Debug.LogError("EventManagerが存在しません。");
            return;
        }
        Bind();

        OnStartEvent();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdateEvent();

        // イベント実行中
        if (EventStatus == eEventStatus.Running)
        {
            TriggerEvent();
        }
    }

    /// <summary>
    /// イベントの終了処理
    /// </summary>
    private void FinishEvent()
    {
        SetIsUnitMove(true); // Unitの移動を有効にする

        EventStatus = _isTriggeredOnce ? eEventStatus.Triggered : eEventStatus.NotTriggered;
        if (_isUpStoryLayer)
        {
            StoryManager.Instance.CurrentStoryLayer++;
        }
        OnFinishEvent();

#if DEBUG_MODE
        Debug.Log($"イベント: {_event} が終了しました");
#endif
    }

    private void Bind()
    {
        onTriggerEvent
            .Subscribe(_ =>
            {
                EventStatus = eEventStatus.Running;
                SetIsUnitMove(false);
            })
            .AddTo(_disposable);

        onFinishEvent
            .Subscribe(_ =>
            {
                FinishEvent();
            })
            .AddTo(_disposable);
    }

    /// <summary>
    /// Unitの移動を有効/無効にする
    /// </summary>
    /// <param name="isUnitMove"> 有効/無効 </param>
    private void SetIsUnitMove(bool isUnitMove)
    {
        // 全てのUnitControllerを取得
        List<AbstractUnitController> unitControllers = new List<AbstractUnitController>(FindObjectsByType<AbstractUnitController>(FindObjectsSortMode.None));

        foreach (AbstractUnitController unitController in unitControllers)
        {
            unitController.IsEnabled = isUnitMove;
        }
    }

    /// <summary>
    /// イベントの初期化処理(Start()の代わり)
    /// </summary>
    public virtual void OnStartEvent()
    {
    }

    /// <summary>
    /// イベントの更新処理(Update()の代わり)
    /// </summary>
    public virtual void OnUpdateEvent()
    {
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
    /// イベントが終了したか
    /// </summary>
    /// <returns> 終了したか </returns>
    public abstract bool IsFinishEvent();

    public virtual void OnFinishEvent()
    {
    }

    /// <summary>
    /// イベントの種類(ReadOnly)
    /// </summary>
    protected eEvent Event
    {
        get { return _event; }
    }

    /// <summary>
    /// イベントの状態(ReadOnly)
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
    /// 保存するイベントのデータ
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
    /// 1度しか実行しないか
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
    /// 有効か
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
    /// デフォルトのEventData
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

    private void OnDestroy()
    {
        _disposable?.Dispose();
    }
}
