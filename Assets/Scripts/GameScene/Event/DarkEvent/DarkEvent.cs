using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DarkEvent : AbstractEvent
{
    // フェードする種類
    enum eFadeType
    {
        InOut,
        In,
        Out
    }

    [Header("フェードの種類")]
    [SerializeField] private eFadeType _fadeType;

    [Header("シーンに入ったらすぐ実行するか")]
    [SerializeField] private bool _isTriggerForce = false;

    [Header("アニメーションの時間(s)")]
    [SerializeField] private float _darkTime = 0;

    [SerializeField] private GameObject _darkPrefab;

    private bool _isInEvent = false;
    private GameObject _canvas;
    private GameObject _darkObj;
    private Image _imageRenderer;

    private bool _hasDisplayedDark = false;
    private bool _hasFinished = false;
    public override void OnStartEvent()
    {
        _canvas = GameObject.FindWithTag("UICanvas");
        if (_canvas == null)
        {
            Debug.LogError("Canvasが見つかりませんでした。");
            return;
        }

        if (_darkPrefab == null)
        {
            Debug.LogError("_darkPrefabがnullです。");
            return;
        }

        _darkObj = GameObject.FindWithTag("Dark");
        if (_darkObj != null)
        {
            _imageRenderer = _darkObj.GetComponent<Image>();
        }
        else
        {
            _darkObj = Instantiate(_darkPrefab, _canvas.transform);
            _imageRenderer = _darkObj.GetComponent<Image>();

            // 初期状態は非表示
            _darkObj.SetActive(false);
        }
        if (_imageRenderer == null)
        {
            Debug.LogError("Image コンポーネントが見つかりません。PrefabにImageコンポーネントを追加してください。");
            return;
        }
    }

    public override void TriggerEvent()
    {
        if (_canvas == null || _darkObj == null || _imageRenderer == null)
        {
            Debug.LogError("必要なオブジェクトが取得できていません。OnStartEventが正常に実行されていない可能性があります。");
            return;
        }

        if (!_hasDisplayedDark)
        {
            _darkObj.SetActive(true);

            Color color = _imageRenderer.color;

            Sequence fadeSequence = DOTween.Sequence();
            switch (_fadeType)
            {
                case eFadeType.InOut:
                    _imageRenderer.color = new Color(color.r, color.g, color.b, 0f); // 透明から開始
                    fadeSequence.Append(_imageRenderer.DOFade(1f, _darkTime / 2));
                    fadeSequence.AppendInterval(0.5f);
                    fadeSequence.Append(_imageRenderer.DOFade(0f, 1f));
                    break;

                case eFadeType.In:
                    _imageRenderer.color = new Color(color.r, color.g, color.b, 0f); // 透明から開始
                    fadeSequence.Append(_imageRenderer.DOFade(1f, _darkTime));
                    fadeSequence.AppendInterval(0.5f);
                    break;

                case eFadeType.Out:
                    _imageRenderer.color = new Color(color.r, color.g, color.b, 1f); // 不透明から開始
                    fadeSequence.Append(_imageRenderer.DOFade(0f, _darkTime));
                    fadeSequence.AppendInterval(0.5f);
                    break;
            }

            fadeSequence.OnComplete(() =>
            {
                // InOut と Out の場合はオブジェクトを非表示にする
                if (_fadeType == eFadeType.InOut || _fadeType == eFadeType.Out)
                {
                    _darkObj.SetActive(false);
                }
                _hasFinished = true;
            });

            _hasDisplayedDark = true;
        }
    }

    public override bool IsFinishEvent()
    {
        return _hasFinished;
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
