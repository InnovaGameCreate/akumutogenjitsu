using UnityEngine;
using R3;

public class Floors2And3StairsPasswordGimmick : AbstractEvent
{
    [Header("階段の壁")]
    [SerializeField] private GameObject _wall;

    [Header("パスワード")]
    [SerializeField] private string _password;

    // Playerが入力したパスワード
    private string _userInput;

    private bool _isPlayerIn = false;
    private bool _hasFinished = false;

    public override void OnStartEvent()
    {
        if (_wall == null)
        {
            Debug.LogError("壁オブジェクトが設定されていません。");
        }

        if (!Enabled)
        {
            _wall.SetActive(false);
        }

        PlayerInput.Instance.OnPerformed(PlayerInput.Instance.Input.Base.Interact)
            .Where(ctx => ctx.ReadValueAsButton() && _isPlayerIn)
            .Subscribe(_ =>
            {
                onTriggerEvent.OnNext(Unit.Default);
            })
            .AddTo(_disposable);
    }

    public override void TriggerEvent()
    {
        // TODO: UI����p�X���[�h���͂��󂯎�鏈���ɒu��������
        _userInput = "1625";

        if (_userInput == _password)
        {
            if (_wall != null)
            {
                _wall.SetActive(false);
                // イベントが完全に無効化されるためにTriggeredステータスに設定
                EventStatus = eEventStatus.Triggered;
                TriggerOnce = true;
                Debug.Log("ようやくここで1階に...");
            }
        }
        else
        {
            Debug.Log("パスワードが間違っています");
        }

        _hasFinished = true;
    }

    private bool IsFinishEvent()
    {
        return _hasFinished;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isPlayerIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isPlayerIn = false;
        }
    }
}

