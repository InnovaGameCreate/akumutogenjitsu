using System.Collections.Generic;
using R3;
using UnityEngine;

public class EventQueue : MonoBehaviour
{
    [Header("イベントキュー")]
    [SerializeField] private List<GameObject> _eventQueue = new();
    private List<AbstractEvent> _allEvents = new();

    [Header("１度だけ実行する")]
    [SerializeField] private bool _isTriggerOnce = false;

    [Header("シーン読み込み時に即座に実行")]
    [SerializeField] private bool _isTriggerForce = false;

    private int _currentEventIndex = 0;
    private bool _isQueueCompleted = false;
    private bool _isCurrentEventRunning = false;

    private CompositeDisposable _disposables = new();

    void Start()
    {
        InitializeQueue();

        if (_isTriggerForce && _allEvents.Count > 0)
        {
            StartNextEvent();
        }
    }

    void Update()
    {
        if (_isQueueCompleted || _allEvents.Count == 0)
        {
            return;
        }

        if (_currentEventIndex >= _allEvents.Count)
        {
            OnQueueCompleted();
            return;
        }

        AbstractEvent currentEvent = _allEvents[_currentEventIndex];
        if (IsEventCompleted(currentEvent) && _isCurrentEventRunning)
        {
            _isCurrentEventRunning = false;
            MoveToNextEvent();
        }
    }

    /// <summary>
    /// イベントキューを初期化
    /// </summary>
    private void InitializeQueue()
    {
        _allEvents.Clear();
        _currentEventIndex = 0;
        _isQueueCompleted = false;
        _isCurrentEventRunning = false;

        foreach (var eventObj in _eventQueue)
        {
            if (eventObj == null)
            {
                Debug.LogWarning("[EventQueue] イベントオブジェクトがnullです。スキップします。");
                continue;
            }

            AbstractEvent ev = eventObj.GetComponent<AbstractEvent>();
            if (ev == null)
            {
                Debug.LogWarning($"[EventQueue] {eventObj.name} にAbstractEventがアタッチされていません。");
                continue;
            }

            _allEvents.Add(ev);
        }

        if (_allEvents.Count == 0)
        {
            Debug.LogWarning("[EventQueue] 有効なイベントが一つもありません。");
            gameObject.SetActive(false);
            return;
        }

        Debug.Log($"[EventQueue] {_allEvents.Count}個のイベントを読み込みました。");
    }

    /// <summary>
    /// イベントが完了しているか判定
    /// </summary>
    private bool IsEventCompleted(AbstractEvent ev)
    {
        if (ev == null)
        {
            return true;
        }

        return ev.EventStatus == eEventStatus.Triggered;
    }

    /// <summary>
    /// 次のイベントに移動
    /// </summary>
    private void MoveToNextEvent()
    {
        _currentEventIndex++;

        if (_currentEventIndex >= _allEvents.Count)
        {
            OnQueueCompleted();
        }
        else
        {
            StartNextEvent();
        }
    }

    /// <summary>
    /// 次のイベントを開始
    /// </summary>
    private void StartNextEvent()
    {
        if (_currentEventIndex >= _allEvents.Count)
        {
            return;
        }

        AbstractEvent nextEvent = _allEvents[_currentEventIndex];

        if (nextEvent == null)
        {
            Debug.LogWarning($"[EventQueue] イベント[{_currentEventIndex}]がnullです。");
            MoveToNextEvent();
            return;
        }

        Debug.Log($"[EventQueue] イベント実行: {nextEvent.gameObject.name} ({_currentEventIndex + 1}/{_allEvents.Count})");

        _isCurrentEventRunning = true;
        nextEvent.TriggerEventForce();
    }

    /// <summary>
    /// キュー完了時の処理
    /// </summary>
    private void OnQueueCompleted()
    {
        _isQueueCompleted = true;

        if (_isTriggerOnce)
        {
            Debug.Log("[EventQueue] キュー完了。オブジェクトを無効化します。");
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("[EventQueue] キュー完了。ループのため再初期化します。");
            InitializeQueue();

            if (_isTriggerForce && _allEvents.Count > 0)
            {
                StartNextEvent();
            }
        }
    }

    void OnDestroy()
    {
        _disposables?.Dispose();
    }
}
