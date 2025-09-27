using UnityEngine;

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

    public override bool IsFinishEvent()
    {
        if (_hasFinished)
        {
            _hasFinished = false;
            return true;
        }
        return false;
    }

    public override bool IsTriggerEvent()
    {
        return (_isInEvent && Input.GetKeyDown(KeyCode.Z)) || _isTriggerForce;
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
