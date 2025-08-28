using System.Collections.Generic;
using System.Linq.Expressions;
using R3;
using UnityEngine;

public class EventQueue : MonoBehaviour
{
    [Header("イベントキュー")]
    [SerializeField] private List<GameObject> _eventQueue = new();
    private List<AbstractEvent> _allEvents = new();
    [Header("１度だけ実行する")]
    [SerializeField] private bool _isTriggerOnce;

    private int _currentEventIndex = 0;

    private readonly CompositeDisposable _disposable = new();

    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        AbstractEvent currentEvent = _allEvents[_currentEventIndex];
        if (currentEvent.EventStatus == eEventStatus.Triggered)
        {
            currentEvent.Enabled = false;

            if (_currentEventIndex + 1 < _allEvents.Count)
            {
                _currentEventIndex++;
                AbstractEvent nextEvent = _allEvents[_currentEventIndex];
                nextEvent.gameObject.SetActive(true);
                nextEvent.Initialized
                    .Where(isInit => isInit)
                    .Take(1)
                    .Subscribe(_ =>
                    {
                        nextEvent.TriggerEvent();
                        nextEvent.EventStatus = eEventStatus.Running;
                    })
                    .AddTo(_disposable);
            }
            else if (_isTriggerOnce)
            {
                Initialize();
            }
        }
    }

    /// <summary>
    /// イベントのオブジェクトをチェックして最初の要素をActiveにする
    /// </summary>
    private void Initialize()
    {
        // イベントは全て一度だけ実行するようにする
        foreach (var eventObj in _eventQueue)
        {
            AbstractEvent ev = eventObj.GetComponent<AbstractEvent>();
            if (ev == null)
            {
                Debug.Log("EventQueueにAbstractEventをアサインしていないGameObjectが指定されています。");
                continue;
            }
            ev.TriggerOnce = true;
            _allEvents.Add(ev);
            ev.gameObject.SetActive(false);
        }
        _currentEventIndex = 0;
        _allEvents[0].gameObject.SetActive(true);
    }
}
