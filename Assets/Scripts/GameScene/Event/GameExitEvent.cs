using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

public class GameExitEvent : AbstractEvent
{
    [Header("触れた状態での操作")]
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private KeyCode _triggerKey = KeyCode.Z; // トリガーキー

    [Header("ゲーム終了設定")]
    [SerializeField] private float _fadeOutDuration = 2f; // フェードアウト時間
    [SerializeField] private bool _showConfirmDialog = true; // 確認ダイアログを表示するか
    [SerializeField] private KeyCode _confirmKey = KeyCode.Y; // 確認キー（Yes）
    [SerializeField] private KeyCode _cancelKey = KeyCode.N; // キャンセルキー（No）

    private bool _playerTouching = false; // プレイヤーが触れているか
    private bool _isEventFinished = false;
    private GameObject _exitCanvas;
    private UnityEngine.UI.Image _fadeImage;
    private UnityEngine.UI.Text _confirmText;
    private bool _isShowingConfirm = false;

    public override void OnStartEvent()
    {
        base.OnStartEvent();
        CreateExitCanvas();
    }

    public override void OnUpdateEvent()
    {
        base.OnUpdateEvent();

        // 確認ダイアログ表示中の処理
        if (_isShowingConfirm)
        {
            if (Input.GetKeyDown(_confirmKey))
            {
                // Yキーでゲーム終了
                StartCoroutine(ExitGame());
            }
            else if (Input.GetKeyDown(_cancelKey))
            {
                // Nキーでキャンセル
                CancelExit();
            }
        }
        // プレイヤーがオブジェクトに触れた状態でZキーを押した場合
        else if (_playerTouching && Input.GetKeyDown(_triggerKey))
        {
            // イベントトリガー条件を満たしているかチェック
            if (CanTriggerEvent())
            {
                TriggerEvent();
            }
        }
    }

    /// <summary>
    /// イベントをトリガーできるかチェック
    /// </summary>
    private bool CanTriggerEvent()
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

        return true;
    }

    /// <summary>
    /// イベントのトリガーの条件
    /// 手動でキーを押した場合のみトリガー
    /// </summary>
    /// <returns>イベントをトリガーするか</returns>
    public override bool IsTriggerEvent()
    {
        // このイベントは手動キー入力でのみトリガーするため、常にfalse
        return false;
    }

    /// <summary>
    /// イベントをトリガーする
    /// </summary>
    public override void TriggerEvent()
    {
        Debug.Log($"ゲーム終了イベントが発生しました: {Event}");

        // イベント開始時の処理
        OnEventTriggered();
    }

    /// <summary>
    /// イベントが終了したか
    /// </summary>
    /// <returns>終了したか</returns>
    public override bool IsFinishEvent()
    {
        return _isEventFinished;
    }

    /// <summary>
    /// イベントがトリガーされた時の処理
    /// </summary>
    protected virtual void OnEventTriggered()
    {
        if (_showConfirmDialog)
        {
            ShowConfirmDialog();
        }
        else
        {
            // 確認なしで即座にゲーム終了
            StartCoroutine(ExitGame());
        }
    }

    /// <summary>
    /// 確認ダイアログを表示
    /// </summary>
    private void ShowConfirmDialog()
    {
        _isShowingConfirm = true;

        // キャンバスをアクティブにする
        if (_exitCanvas != null)
        {
            _exitCanvas.SetActive(true);
        }

        // 確認メッセージを表示
        if (_confirmText != null)
        {
            _confirmText.text = $"ゲームを終了しますか？\n\n[{_confirmKey}] はい / [{_cancelKey}] いいえ";
        }
    }

    /// <summary>
    /// ゲーム終了処理
    /// </summary>
    private IEnumerator ExitGame()
    {
        _isShowingConfirm = false;

        // 確認テキストを変更
        if (_confirmText != null)
        {
            _confirmText.text = "ゲームを終了しています...";
        }

        // フェードアウト
        yield return StartCoroutine(FadeOut());

        // ゲーム終了
        Debug.Log("ゲームを終了します");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

        _isEventFinished = true;
    }

    /// <summary>
    /// 終了をキャンセル
    /// </summary>
    private void CancelExit()
    {
        _isShowingConfirm = false;

        // キャンバスを非アクティブにする
        if (_exitCanvas != null)
        {
            _exitCanvas.SetActive(false);
        }

        _isEventFinished = true;
    }

    /// <summary>
    /// フェードアウト処理
    /// </summary>
    private IEnumerator FadeOut()
    {
        if (_fadeImage == null) yield break;

        float elapsedTime = 0f;
        Color color = _fadeImage.color;

        while (elapsedTime < _fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / _fadeOutDuration);
            color.a = alpha;
            _fadeImage.color = color;
            yield return null;
        }

        // 最終値を確実に設定
        color.a = 1f;
        _fadeImage.color = color;
    }

    /// <summary>
    /// ゲーム終了用のCanvasを作成
    /// </summary>
    private void CreateExitCanvas()
    {
        // 既に存在する場合は何もしない
        if (_exitCanvas != null) return;

        // Canvasを作成
        _exitCanvas = new GameObject("ExitCanvas");
        Canvas canvas = _exitCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000; // 最前面に表示

        // CanvasScaler追加
        UnityEngine.UI.CanvasScaler canvasScaler = _exitCanvas.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasScaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);

        // GraphicRaycaster追加
        _exitCanvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // フェード用の黒い背景パネルを作成
        GameObject fadePanel = new GameObject("FadePanel");
        fadePanel.transform.SetParent(_exitCanvas.transform, false);
        _fadeImage = fadePanel.AddComponent<UnityEngine.UI.Image>();
        _fadeImage.color = new Color(0, 0, 0, 0); // 透明な黒

        // パネルを画面全体に拡大
        RectTransform fadeRect = fadePanel.GetComponent<RectTransform>();
        fadeRect.anchorMin = Vector2.zero;
        fadeRect.anchorMax = Vector2.one;
        fadeRect.offsetMin = Vector2.zero;
        fadeRect.offsetMax = Vector2.zero;

        // 確認用テキストを作成
        GameObject textObject = new GameObject("ConfirmText");
        textObject.transform.SetParent(_exitCanvas.transform, false);
        _confirmText = textObject.AddComponent<UnityEngine.UI.Text>();
        _confirmText.text = "";
        _confirmText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        _confirmText.fontSize = 48;
        _confirmText.color = Color.white;
        _confirmText.alignment = TextAnchor.MiddleCenter;

        // テキストを画面中央に配置
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        // 初期状態では非アクティブ
        _exitCanvas.SetActive(false);
    }

    public override void OnFinishEvent()
    {
        base.OnFinishEvent();
        _isEventFinished = false;
        _isShowingConfirm = false;

        if (_exitCanvas != null)
        {
            _exitCanvas.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (_exitCanvas != null)
        {
            DestroyImmediate(_exitCanvas);
        }
    }

    // プレイヤーがオブジェクトに触れた時
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_playerTag))
        {
            _playerTouching = true;
            Debug.Log("ゲーム終了オブジェクトに触れました。Zキーで終了できます。");
        }
    }

    // プレイヤーがオブジェクトから離れた時
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(_playerTag))
        {
            _playerTouching = false;
            Debug.Log("ゲーム終了オブジェクトから離れました。");
        }
    }

    /// <summary>
    /// 手動でゲーム終了イベントを開始する
    /// </summary>
    public void StartExitEvent()
    {
        TriggerEvent();
    }

    public void SetEventFinished(bool finished = true)
    {
        _isEventFinished = finished;
    }
}
