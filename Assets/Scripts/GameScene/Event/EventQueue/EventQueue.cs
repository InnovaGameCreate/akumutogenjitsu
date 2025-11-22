using System.Collections.Generic;
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

    public override void OnStartEvent()
    {
        PlayerInput.Instance.OnPerformed(PlayerInput.Instance.Input.Base.Interact)
            .Where(ctx => ctx.ReadValueAsButton() && _isInEvent && EventStatus == eEventStatus.NotTriggered)
            .Subscribe(_ =>
            {
                // 最初は手動で実行
                _currentEvent.TriggerEventForce();
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
            _currentEvent.TriggerEventForce();
            onTriggerEvent.OnNext(Unit.Default);
        }
    }

    public override void TriggerEvent()
    {
        if (_currentEventIndex >= _allEvents.Count)
        {
            onFinishEvent.OnNext(Unit.Default);
        }

        if (_currentEvent.EventStatus != eEventStatus.Running)
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
