using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DarkEvent : AbstractEvent
{
    [Header("シーンに入ったらすぐ実行するか")]
    [SerializeField] private bool _isTriggerForce = false;

    [Header("暗転している時間(s)")]
    [SerializeField] private float _darkTime = 0;

    [SerializeField] private GameObject _darkPrefab;

    private bool _isInEvent = false;
    private GameObject _canvas;

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
    }

    public override void TriggerEvent()
    {
        if (_canvas == null)
        {
            Debug.LogError("Canvasが取得できていません。OnStartEventが正常に実行されていない可能性があります。");
            return;
        }

        if (!_hasDisplayedDark)
        {
            GameObject darkObj = Instantiate(_darkPrefab, _canvas.transform);

            Image imageRenderer = darkObj.GetComponent<Image>();
            if (imageRenderer == null)
            {
                Debug.LogError("Image コンポーネントが見つかりません。PrefabにImageコンポーネントを追加してください。");
                return;
            }

            Color color = imageRenderer.color;
            imageRenderer.color = new Color(color.r, color.g, color.b, 0f);

            Sequence fadeSequence = DOTween.Sequence();
            fadeSequence.Append(imageRenderer.DOFade(1f, 1f));
            fadeSequence.AppendInterval(0.5f);
            fadeSequence.Append(imageRenderer.DOFade(0f, 1f));

            fadeSequence.OnComplete(() =>
            {
                if (darkObj != null)
                {
                    Destroy(darkObj);
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
        return _isInEvent || _isTriggerForce;
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
