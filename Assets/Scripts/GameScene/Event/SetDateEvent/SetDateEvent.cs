using UnityEngine;
using R3;
using UnityEngine.InputSystem;

public class SetDateEvent : AbstractEvent
{
    [Header("シーンに入ったらすぐに実行するか")]
    [SerializeField] private bool _isTriggerForce = false;
    [Header("変更後の日付")]
    [SerializeField] private Date _newDate = new Date(9, 6);

    private bool _isInEvent = false;

    private readonly CompositeDisposable _disposables = new CompositeDisposable();

    public override void OnStartEvent()
    {
        // トリガー移設
        if (_isTriggerForce)
        {
            _isTriggerForce = false;
            onTriggerEvent.OnNext(Unit.Default);
        }

        // InputSystemを使用してSetDataEventアクションを監視
        PlayerInput.Instance.OnPerformed(PlayerInput.Instance.Input.Base.Interact)
            .Where(ctx => ctx.ReadValueAsButton() && _isInEvent)
            .Subscribe(_ =>
            {
                onTriggerEvent.OnNext(Unit.Default);
            })
            .AddTo(_disposables);
    }

    private void OnDisable()
    {
        _disposables.Clear();
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }

    public override void TriggerEvent()
    {
        if (!Date.IsValid(_newDate))
        {
            Debug.LogError($"無効な日付が設定されています: {_newDate}");
            onFinishEvent.OnNext(Unit.Default);
            return;
        }

        DateManager.Instance.SetCurrentDate(_newDate);

        // 日付を超えたらストーリーレイヤーを初期化する
        StoryManager.Instance.Initialize();

        onFinishEvent.OnNext(Unit.Default);
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
