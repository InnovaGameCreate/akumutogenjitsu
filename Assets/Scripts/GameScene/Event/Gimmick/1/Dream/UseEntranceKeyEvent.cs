using UnityEngine;

public class UseEntranceKeyEvent : AbstractEvent
{
    [Header("学校の外へ出るイベント")]
    [SerializeField] private MapMoveEvent _mapMoveEvent;

    [Header("EntranceKeyがなかったときのテキストイベント")]
    [SerializeField] private TextEvent _textEvent;

    private bool _isInEvent = false;

    private bool _hasFinished = false;

    public override bool IsFinishEvent()
    {
        return _hasFinished;
    }

    public override bool IsTriggerEvent()
    {
        return _isInEvent && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return));
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
