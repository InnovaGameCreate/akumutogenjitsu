using UnityEngine;
using R3;

public class UseEntranceKeyEvent : AbstractEvent
{
    [Header("学校の外へ出るイベント")]
    [SerializeField] private MapMoveEvent _mapMoveEvent;

    [Header("EntranceKeyがなかったときのテキストイベント")]
    [SerializeField] private TextEvent _textEvent;

    private bool _isInEvent = false;

    private bool _hasFinished = false;

    public override void OnStartEvent()
    {
        PlayerInput.Instance.OnPerformed(PlayerInput.Instance.Input.Base.Interact)
            .Where(ctx => ctx.ReadValueAsButton() && _isInEvent && _textEvent.EventStatus != eEventStatus.Running)
            .Subscribe(_ =>
            {
                onTriggerEvent.OnNext(Unit.Default);
            })
            .AddTo(_disposable);
    }

    private bool IsFinishEvent()
    {
        return _hasFinished;
    }

    public override void TriggerEvent()
    {
        if (_mapMoveEvent == null || _textEvent == null)
        {
            Debug.LogError("イベントが適切にアタッチされていません。");
            _hasFinished = true;
            return;
        }
        if (ItemManager.Instance.GetIsItemOwned(eItem.EntranceKey))
        {
            _mapMoveEvent.TriggerEventForce();
        }
        else
        {
            _textEvent.TriggerEventForce();
        }

        _hasFinished = true;
    }

    public override void OnUpdateEvent()
    {
        // 終了条件チェック
        if (IsFinishEvent())
        {
            onFinishEvent.OnNext(Unit.Default);
        }
    }

    public override void OnFinishEvent()
    {
        _hasFinished = false;
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
}
