using UnityEngine;
using R3;

public class SetDateEvent : AbstractEvent
{
    [Header("シーンに入ったらすぐに実行するか")]
    [SerializeField] private bool _isTriggerForce = false;
    [Header("変更後の日付")]
    [SerializeField] private Date _newDate = new Date(9, 6);

    private bool _isInEvent = false;

    private bool _hasFinished = false;

    public override void TriggerEvent()
    {
        if (!Date.IsValid(_newDate))
        {
            _hasFinished = true;
            Debug.LogError($"無効な日付が設定されています: {_newDate}");
            return;
        }

        DateManager.Instance.SetCurrentDate(_newDate);


        // 日付を超えたらストーリーレイヤーを初期化する
        StoryManager.Instance.Initialize();

        _hasFinished = true;
    }

    private bool IsFinishEvent()
    {
        if (EventStatus == eEventStatus.Triggered)
        {
            _hasFinished = false;
        }
        return _hasFinished;
    }

    private bool IsTriggerEvent()
    {
        return (_isInEvent && Input.GetKeyDown(KeyCode.Z)) || _isTriggerForce;
    }

    public override void OnUpdateEvent()
    {
        // トリガー条件チェック
        if (IsTriggerEvent())
        {
            _isTriggerForce = false;
            onTriggerEvent.OnNext(Unit.Default);
        }

        // 終了条件チェック
        if (IsFinishEvent())
        {
            onFinishEvent.OnNext(Unit.Default);
        }
    }

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
