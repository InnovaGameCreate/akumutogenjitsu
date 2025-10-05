using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

public class CollisionTriggerEvent : AbstractEvent
{
    [Header("衝突設定")]
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private bool _useTrigger = true; // Triggerとして使用するか（falseの場合はCollision）

    [Header("表示画像設定")]
    [SerializeField] private Sprite _displaySprite; // 表示する画像
    [SerializeField] private KeyCode _closeKey = KeyCode.Z; // 閉じるキー
    [SerializeField] private float _fadeInDuration = 0.5f; // フェードイン時間
    [SerializeField] private float _fadeOutDuration = 0.5f; // フェードアウト時間

    private bool _hasCollided = false;
    private bool _isEventFinished = false;
    private GameObject _imageCanvas;
    private UnityEngine.UI.Image _displayImage;
    private bool _isDisplayingImage = false;

    public override void OnStartEvent()
    {
        base.OnStartEvent();
        CreateImageCanvas();
    }

    public override void OnUpdateEvent()
    {
        base.OnUpdateEvent();

        // 画像表示中の処理
        if (_isDisplayingImage)
        {
            // Zキーで閉じる
            if (Input.GetKeyDown(_closeKey))
            {
                StartCoroutine(CloseImage());
            }
        }

        // トリガー条件チェック
        if (IsTriggerEvent())
        {
            onTriggerEvent.OnNext(Unit.Default);
        }

        // 終了条件チェック
        if (IsFinishEvent())
        {
            onFinishEvent.OnNext(Unit.Default);
        }
    }

    /// <summary>
    /// イベントのトリガーの条件
    /// プレイヤーと衝突した場合にトリガーする
    /// </summary>
    /// <returns>イベントをトリガーするか</returns>
    private bool IsTriggerEvent()
    {
        // EventDataが存在しない場合は作成
        try
        {
            var testData = EventManager.Instance.GetEventData(EventId);
        }
        catch (System.Collections.Generic.KeyNotFoundException)
        {
            EventManager.Instance.SetEventData(EventId, DefaultEventData);
        }

        // StoryLayerのチェック
        if (StoryLayer > 0 && StoryManager.Instance.CurrentStoryLayer < StoryLayer)
        {
            return false;
        }

        // 有効でない場合はトリガーしない
        if (!Enabled)
        {
            return false;
        }

        // 一度だけ実行する設定で、既に実行済みの場合はトリガーしない
        if (TriggerOnce && EventStatus == eEventStatus.Triggered)
        {
            return false;
        }

        return _hasCollided;
    }

    /// <summary>
    /// イベントをトリガーする
    /// </summary>
    public override void TriggerEvent()
    {
        Debug.Log($"画像表示イベントが発生しました: {Event}");

        // イベント開始時の処理
        OnEventTriggered();
    }

    /// <summary>
    /// イベントが終了したか
    /// </summary>
    /// <returns>終了したか</returns>
    private bool IsFinishEvent()
    {
        return _isEventFinished;
    }

    /// <summary>
    /// イベントがトリガーされた時の処理（継承先でオーバーライド可能）
    /// </summary>
    protected virtual void OnEventTriggered()
    {
        if (_displaySprite != null)
        {
            StartCoroutine(ShowImage());
        }
        else
        {
            Debug.LogWarning("表示する画像が設定されていません");
            _isEventFinished = true;
        }
    }

    /// <summary>
    /// 画像を表示するコルーチン
    /// </summary>
    private IEnumerator ShowImage()
    {
        _isDisplayingImage = true;

        // キャンバスをアクティブにする
        if (_imageCanvas != null)
        {
            _imageCanvas.SetActive(true);
        }

        // 画像を設定
        if (_displayImage != null && _displaySprite != null)
        {
            _displayImage.sprite = _displaySprite;
        }

        // フェードイン
        yield return StartCoroutine(FadeImage(0f, 1f, _fadeInDuration));
    }

    /// <summary>
    /// 画像を閉じるコルーチン
    /// </summary>
    private IEnumerator CloseImage()
    {
        // フェードアウト
        yield return StartCoroutine(FadeImage(1f, 0f, _fadeOutDuration));

        // キャンバスを非アクティブにする
        if (_imageCanvas != null)
        {
            _imageCanvas.SetActive(false);
        }

        _isDisplayingImage = false;
        _isEventFinished = true;
    }

    /// <summary>
    /// イメージのフェード処理
    /// </summary>
    private IEnumerator FadeImage(float startAlpha, float endAlpha, float duration)
    {
        if (_displayImage == null) yield break;

        float elapsedTime = 0f;
        Color color = _displayImage.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            color.a = alpha;
            _displayImage.color = color;
            yield return null;
        }

        // 最終値を確実に設定
        color.a = endAlpha;
        _displayImage.color = color;
    }

    /// <summary>
    /// 画像表示用のCanvasを作成
    /// </summary>
    private void CreateImageCanvas()
    {
        // 既に存在する場合は何もしない
        if (_imageCanvas != null) return;

        // Canvasを作成
        _imageCanvas = new GameObject("ImageCanvas");
        Canvas canvas = _imageCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000; // 最前面に表示

        // CanvasScaler追加
        UnityEngine.UI.CanvasScaler canvasScaler = _imageCanvas.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasScaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);

        // GraphicRaycaster追加
        _imageCanvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // 背景パネルを作成
        GameObject backgroundPanel = new GameObject("BackgroundPanel");
        backgroundPanel.transform.SetParent(_imageCanvas.transform, false);
        UnityEngine.UI.Image backgroundImage = backgroundPanel.AddComponent<UnityEngine.UI.Image>();
        backgroundImage.color = new Color(0, 0, 0, 0.8f); // 半透明の黒背景

        // パネルを画面全体に拡大
        RectTransform backgroundRect = backgroundPanel.GetComponent<RectTransform>();
        backgroundRect.anchorMin = Vector2.zero;
        backgroundRect.anchorMax = Vector2.one;
        backgroundRect.offsetMin = Vector2.zero;
        backgroundRect.offsetMax = Vector2.zero;

        // スプライト表示用のImageを作成
        GameObject imageObject = new GameObject("DisplayImage");
        imageObject.transform.SetParent(_imageCanvas.transform, false);
        _displayImage = imageObject.AddComponent<UnityEngine.UI.Image>();
        _displayImage.preserveAspect = true;

        // イメージを画面中央に配置
        RectTransform imageRect = imageObject.GetComponent<RectTransform>();
        imageRect.anchorMin = new Vector2(0.5f, 0.5f);
        imageRect.anchorMax = new Vector2(0.5f, 0.5f);
        imageRect.pivot = new Vector2(0.5f, 0.5f);
        imageRect.sizeDelta = new Vector2(2000, 1200); // デフォルトサイズ

        // 初期状態では非アクティブ
        _imageCanvas.SetActive(false);
    }

    public override void OnFinishEvent()
    {
        base.OnFinishEvent();
        _hasCollided = false;
        _isEventFinished = false;
        _isDisplayingImage = false;

        if (_imageCanvas != null)
        {
            _imageCanvas.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (_imageCanvas != null)
        {
            DestroyImmediate(_imageCanvas);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_useTrigger && other.CompareTag(_playerTag))
        {
            _hasCollided = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_useTrigger && collision.gameObject.CompareTag(_playerTag))
        {
            _hasCollided = true;
        }
    }

    public void SetEventFinished(bool finished = true)
    {
        _isEventFinished = finished;
    }
}
