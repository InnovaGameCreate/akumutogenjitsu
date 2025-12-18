using R3;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class SpriteEvent : AbstractEvent
{
    [Header("表示したい画像")]
    [SerializeField] private Sprite _sprite;

    [Header("フェード設定")]
    [SerializeField] private float _fadeDuration = 0.5f;

    private GameObject _canvas;

    [SerializeField] private Image _spriteImage;

    private Image _imgObj;

    private bool _isInEvent = false;
    private bool _isDisplaying = false;

    public override void OnStartEvent()
    {
        PlayerInput.Instance.OnPerformed(PlayerInput.Instance.Input.Base.Interact)
            .Where(ctx => ctx.ReadValueAsButton() && _isInEvent && !_isDisplaying)
            .Subscribe(_ =>
            {
                onTriggerEvent.OnNext(Unit.Default);
            })
            .AddTo(_disposable);

        PlayerInput.Instance.OnPerformed(PlayerInput.Instance.Input.SpriteEvent.Close)
            .Where(ctx => ctx.ReadValueAsButton() && _isDisplaying)
            .Subscribe(_ =>
            {
                FadeOutAndFinish();
            })
            .AddTo(_disposable);
    }

    public override void TriggerEvent()
    {
        if (_isDisplaying)
        {
            return;
        }

        _canvas = GameObject.FindWithTag("UICanvas");
        if (_canvas == null)
        {
            Debug.LogError("_canvasがnullです");
            return;
        }
        if (_sprite == null)
        {
            Debug.LogError("_spriteが指定されていません。");
            return;
        }

        // Imageの生成
        _spriteImage.sprite = _sprite;
        _imgObj = Instantiate(_spriteImage);
        _imgObj.transform.SetParent(_canvas.transform, false);

        // 初期状態を透明に設定
        Color color = _imgObj.color;
        color.a = 0f;
        _imgObj.color = color;

        // InputSystemの変更
        PlayerInput.Instance.Input.Base.Disable();
        PlayerInput.Instance.Input.SpriteEvent.Enable();

        _isDisplaying = true;

        // フェードイン
        _imgObj.DOFade(1f, _fadeDuration)
            .SetEase(Ease.InOutQuad);
    }

    private void FadeOutAndFinish()
    {
        if (_imgObj == null)
        {
            return;
        }

        // フェードアウト後にイベント終了
        _imgObj.DOFade(0f, _fadeDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                onFinishEvent.OnNext(Unit.Default);
            });
    }

    public override void OnFinishEvent()
    {
        if (_imgObj != null)
        {
            _imgObj.DOKill(); // 進行中のTweenを停止
            Destroy(_imgObj.gameObject);
            _imgObj = null;
        }

        PlayerInput.Instance.Input.SpriteEvent.Disable();
        PlayerInput.Instance.Input.Base.Enable();

        _isDisplaying = false;
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
