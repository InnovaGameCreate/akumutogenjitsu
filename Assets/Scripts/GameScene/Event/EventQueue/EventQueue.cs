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
    [SerializeField] private bool _isTriggerForce = false;

    private int _currentEventIndex = 0;

    void Start()
    {
        Initialize();

        if (_isTriggerForce)
        {
            SetupNextEvent(_allEvents[0]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentEventIndex > _allEvents.Count)
        {
            return;
        }
        AbstractEvent currentEvent = _allEvents[_currentEventIndex];
        if (currentEvent.EventStatus == eEventStatus.Triggered || !currentEvent.Enabled)
        {
            currentEvent.Enabled = false;

            if (_currentEventIndex + 1 < _allEvents.Count)
            {
                _currentEventIndex++;
                AbstractEvent nextEvent = _allEvents[_currentEventIndex];
                SetupNextEvent(nextEvent);
            }
            else if (!_isTriggerOnce)
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
            if (ev == null || ev.EventStatus == eEventStatus.Triggered)
            {
                continue;
            }
            _allEvents.Add(ev);
            ev.Enabled = false;
        }

        if (_allEvents.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        _currentEventIndex = 0;
        _allEvents[0].Enabled = true;
    }

    private void SetupNextEvent(AbstractEvent nextEvent)
    {
        nextEvent.Enabled = true;
        // イベントを実行する
        nextEvent.TriggerEventForce();
    }
}
