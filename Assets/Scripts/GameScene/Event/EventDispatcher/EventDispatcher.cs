using System.Collections.Generic;
using R3;
using UnityEngine;

public class EventDispatcher : AbstractEvent
{
    [Header("実行するイベント")]
    [SerializeField] private List<GameObject> _eventObjs = new();
    private List<AbstractEvent> _events = new();

    [Header("シーン読み込み時に即座に実行")]
    [SerializeField] private bool _isTriggerForce = false;

    private bool _isInEvent = false;

    public override void OnStartEvent()
    {
        // イベントリスト初期化
        foreach (var obj in _eventObjs)
        {
            if (obj == null)
            {
                Debug.LogWarning("[EventDispatcher] イベントオブジェクトがnullです。");
                continue;
            }

            AbstractEvent evt = obj.GetComponent<AbstractEvent>();
            if (evt == null)
            {
                Debug.LogWarning($"[EventDispatcher] {obj.name} にAbstractEventがアタッチされていません。");
                continue;
            }

            _events.Add(evt);
        }

        PlayerInput.Instance.OnPerformed(PlayerInput.Instance.Input.Base.Interact)
            .Where(ctx => ctx.ReadValueAsButton() && _isInEvent && EventStatus == eEventStatus.NotTriggered)
            .Subscribe(_ =>
            {
                onTriggerEvent.OnNext(Unit.Default);
            })
            .AddTo(_disposable);

        // 強制実行
        if (_isTriggerForce)
        {
            onTriggerEvent.OnNext(Unit.Default);
        }
    }

    public override void TriggerEvent()
    {
        foreach (var evt in _events)
        {
            if (evt == null)
            {
                Debug.LogWarning("[EventDispatcher] イベントがnullです。");
                continue;
            }

            evt.TriggerEventForce();
        }
        onFinishEvent.OnNext(Unit.Default);
    }

    // OnTrigger
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
}
