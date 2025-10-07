using UnityEngine;
using System.Collections.Generic;
using R3;

/// <summary>
/// プロローグイベント - 真っ暗な画面にタイプライター効果でテキスト表示
/// </summary>
public class PrologueEvent : AbstractEvent
{
    [System.Serializable]
    public class PrologueLine
    {
        [Header("表示テキスト")]
        [TextArea(3, 5)]
        public string text = "";

        [Header("表示開始までの待機時間（秒）")]
        [Range(0f, 10f)]
        public float delayBeforeDisplay = 0f;

        [Header("文字を1文字ずつ表示する速度（秒）")]
        [Range(0.01f, 1f)]
        public float typeSpeed = 0.1f;

        [Header("次のテキストまでの待機時間（秒）")]
        [Range(0f, 10f)]
        public float delayAfterDisplay = 0f;
    }

    [Header("プロローグイベント設定")]
    [SerializeField] private bool _isTriggerForce = false;
    [SerializeField] private List<PrologueLine> _prologueLines = new List<PrologueLine>();

    [Header("UI設定")]
    [SerializeField] private GameObject _prologueUIPrefab;

    private bool _isInEvent = false;
    private bool _hasFinished = false;
    private GameObject _prologueUIInstance;
    private ProloguePresenter _prologuePresenter;
    private Canvas _targetCanvas;

    public override void OnStartEvent()
    {
        _targetCanvas = GameObject.FindGameObjectWithTag("UICanvas")?.GetComponent<Canvas>();
        if (_targetCanvas == null)
        {
            Debug.LogError("[PrologueEvent] UICanvasが見つかりません。", this);
        }
    }

    private bool IsTriggerEvent()
    {
        return (_isInEvent && Input.GetKeyDown(KeyCode.Z)) || _isTriggerForce;
    }

    public override void TriggerEvent()
    {
        if (_hasFinished || !CanStartPrologue())
        {
            return;
        }

        CreatePrologueUI();
    }

    public override void OnUpdateEvent()
    {
        // トリガー条件チェック
        if (IsTriggerEvent())
        {
            _isTriggerForce = false;
            onTriggerEvent.OnNext(Unit.Default);
        }

        // プレゼンターがキー入力を処理するため、ここでは完了チェックのみ
        if (_prologuePresenter != null && _prologuePresenter.IsFinished())
        {
            CleanupPrologueUI();
            onFinishEvent.OnNext(Unit.Default);
        }

        // 終了条件チェック
        if (_hasFinished)
        {
            onFinishEvent.OnNext(Unit.Default);
        }
    }

    private bool CanStartPrologue()
    {
        return _targetCanvas != null && _prologueUIPrefab != null &&
               _prologueLines != null && _prologueLines.Count > 0;
    }

    private void CreatePrologueUI()
    {
        if (_prologueUIInstance != null) return;

        _prologueUIInstance = Instantiate(_prologueUIPrefab);
        _prologueUIInstance.transform.SetParent(_targetCanvas.transform, false);
        _prologuePresenter = _prologueUIInstance.GetComponent<ProloguePresenter>();

        if (_prologuePresenter != null)
        {
            _prologuePresenter.Initialize(_prologueLines);
            _prologuePresenter.StartPrologue();
        }
        else
        {
            Debug.LogError("[PrologueEvent] ProloguePresenterが見つかりません。", this);
            CleanupPrologueUI();
            _hasFinished = true;
        }
    }

    private void CleanupPrologueUI()
    {
        if (_prologueUIInstance != null)
        {
            Destroy(_prologueUIInstance);
            _prologueUIInstance = null;
            _prologuePresenter = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInEvent = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInEvent = false;
        }
    }

    private void OnDestroy()
    {
        CleanupPrologueUI();
    }
}
