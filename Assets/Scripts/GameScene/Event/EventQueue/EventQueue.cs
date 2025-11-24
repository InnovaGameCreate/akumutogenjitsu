using System.Collections.Generic;
using NUnit.Framework;
using R3;
using UnityEngine;

public class EventQueue : AbstractEvent
{
    [Header("イベントキュー")]
    [SerializeField] private List<GameObject> _eventQueue = new();
    private List<AbstractEvent> _allEvents = new();

    [Header("シーン読み込み時に即座に実行")]
    [SerializeField] private bool _isTriggerForce = false;

    private int _currentEventIndex = 0;
    private bool _isInEvent = false;

    private bool _hasTriggered = false;

    public override void OnStartEvent()
    {
        PlayerInput.Instance.OnPerformed(PlayerInput.Instance.Input.Base.Interact)
            .Where(ctx => ctx.ReadValueAsButton() && _isInEvent && EventStatus == eEventStatus.NotTriggered)
            .Subscribe(_ =>
            {
                // 最初は手動で実行
                onTriggerEvent.OnNext(Unit.Default);
            })
            .AddTo(_disposable);

        foreach (var obj in _eventQueue)
        {
            AbstractEvent evt = obj.GetComponent<AbstractEvent>();
            if (evt == null)
            {
                continue;
            }

            _allEvents.Add(evt);
        }

        if (_isTriggerForce)
        {
            onTriggerEvent.OnNext(Unit.Default);
        }
    }

    public override void TriggerEvent()
    {
        if (_currentEventIndex >= _allEvents.Count - 1 && _currentEvent.EventStatus != eEventStatus.Running)
        {
            onFinishEvent.OnNext(Unit.Default);
            return;
        }

        if (!_hasTriggered)
        {
            _currentEvent.TriggerEventForce();
            _hasTriggered = true;
            return;
        }

        if (_allEvents[_currentEventIndex].EventStatus != eEventStatus.Running)
        {
            _currentEventIndex++;
            _currentEvent.TriggerEventForce();
        }
    }

    // MARK: OnTrigger
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInEvent = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInEvent = false;
        }
    }

    private AbstractEvent _currentEvent
    {
        get => _allEvents[_currentEventIndex];
    }
}
