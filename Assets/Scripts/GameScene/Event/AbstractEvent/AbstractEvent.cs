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

    private StoryManager _storyManager;
    private EventManager _eventManager;

    // 保存するデータ
    private EventData _eventData = new EventData();

    // 初期化されたか
    private readonly ReactiveProperty<bool> _initialized = new(false);
    public ReadOnlyReactiveProperty<bool> Initialized => _initialized;

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
            _eventData.EventId = value;
        }
    }

    void Start()
    {
        _storyManager = GameObject.FindGameObjectWithTag("StoryMgr").GetComponent<StoryManager>();
        if (_storyManager == null)
        {
            Debug.LogError("StoryManagerが存在しません。");
            return;
        }
        _eventManager = FindAnyObjectByType<EventManager>();
        if (_eventManager == null)
        {
            Debug.LogError("EventManagerが存在しません。");
            return;
        }

        if (_eventManager.HasLoadedEvent(_eventId))
        {
            _eventData = _eventManager.LoadEventData(_eventId);
        }
        else
        {
            // EventDataの初期化
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
        // EventStoryとEventStatusで有効/無効を決める
        if (_storyManager.CurrentStoryLayer == _storyLayer || _storyLayer == 0)
        {
            _eventData.Enabled = _eventData.EventStatus != eEventStatus.Triggered;
        }
        else
        {
            _eventData.Enabled = false;
        }

        // 無効の場合表示を消して処理を止める
        if (_eventData.Enabled == false)
        {
            BasicAnimation animation = gameObject.GetComponent<BasicAnimation>();
            if (animation != null)
            {
                // EventQueueを使ったときSetActive(false);をされ、nullになる可能性がある。
                animation.Enabled = false;
            }
            _eventManager.SaveEventData(_eventId, _eventData);
            return;
        }

        OnUpdateEvent();

        // イベントが実行中はトリガーしない
        if (_eventData.EventStatus != eEventStatus.Running)
        {
            if (IsTriggerEvent())
            {
                SetIsUnitMove(false); // Unitの移動を無効にする

                TriggerEvent();
                _eventData.EventStatus = eEventStatus.Running;
            }
        }

        if (IsFinishEvent() && _eventData.EventStatus == eEventStatus.Running)
        {
            SetIsUnitMove(true); // Unitの移動を有効にする

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
            Debug.Log($"イベント: {_event} が終了しました");
#endif
        }
    }

    /// <summary>
    /// Unitの移動を有効/無効にする
    /// </summary>
    /// <param name="isUnitMove"> 有効/無効 </param>
    private void SetIsUnitMove(bool isUnitMove)
    {
        // 全てのUnitのUnitMoveを取得
        List<UnitMove> units = new List<UnitMove>(FindObjectsByType<UnitMove>(FindObjectsSortMode.None));

        foreach (UnitMove unit in units)
        {
            unit.IsEnabled = isUnitMove;
        }
    }

    /// <summary>
    /// EventDataを適応する
    /// </summary>
    /// <param name="eventData"> EventManagerにあるEventData </param>
    public void InitWithEventData(EventData eventData)
    {
        _eventData = eventData;
        Debug.Log($"type: {_eventData.EventType}, enabled: {_eventData.Enabled}");
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
            return _eventData.EventStatus;
        }
        set
        {
            _eventData.EventStatus = value;
        }
    }

    /// <summary>
    /// 保存するイベントのデータ
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
        get => _eventData.Enabled;
        set => _eventData.Enabled = value;
    }
}
