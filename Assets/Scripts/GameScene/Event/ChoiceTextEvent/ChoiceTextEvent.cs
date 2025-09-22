using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

[Serializable]
public class Choice
{
    [Header("選択肢")]
    [SerializeField]
    private string _choiceText;

    [Header("起きること")]
    [SerializeField] private GameObject _nextEvent;

    [Header("もう一度選択肢を出すか")]
    [SerializeField] private bool _isReturn = false;

    /// <summary>
    /// 選択肢の文章
    /// </summary>
    public string ChoiceText
    {
        get => _choiceText;
        set => _choiceText = value;
    }

    /// <summary>
    /// 選択したときに実行されるイベント
    /// </summary>
    public GameObject NextEvent
    {
        get => _nextEvent;
        set => _nextEvent = value;
    }

    public bool IsReturn
    {
        get => _isReturn;
        set => _isReturn = value;
    }
}

public class ChoiceTextEvent : AbstractEvent
{
    [Header("テキスト")]
    [TextArea(3, 5)]
    [SerializeField] private string _message;

    [Header("選択肢")]
    [SerializeField] private List<Choice> _choices = new();

    [SerializeField] private ChoiceTextEventView _viewPrefab;
    private GameObject _viewObj;
    private ChoiceTextEventView _view;

    private GameObject _canvas;

    private bool _isInEvent = false;
    private bool _isFinish = false;

    private CompositeDisposable _disposable = new();

    public override void OnStartEvent()
    {
        _canvas = GameObject.FindWithTag("UICanvas");
        if (_canvas == null)
        {
            Debug.LogError("Canvasが存在しません。");
            return;
        }
    }

    public override bool IsFinishEvent()
    {
        return _isFinish;
    }

    public override bool IsTriggerEvent()
    {
        return _isInEvent && Input.GetKeyDown(KeyCode.Z);
    }

    public override void TriggerEvent()
    {
        if (_viewObj == null)
        {
            InitializeView();
            PlayerInput.Instance.Input.Base.Disable();
            PlayerInput.Instance.Input.ChoiceTextEvent.Enable();
        }
    }

    public override void OnFinishEvent()
    {
        // InputActionを確実に無効化
        if (PlayerInput.Instance?.Input?.ChoiceTextEvent != null)
        {
            PlayerInput.Instance.Input.ChoiceTextEvent.Disable();
        }

        // 基本のInputActionを再有効化
        if (PlayerInput.Instance?.Input?.Base != null)
        {
            PlayerInput.Instance.Input.Base.Enable();
        }

        _isFinish = false;
        if (_viewObj != null)
        {
            Destroy(_viewObj);
            _viewObj = null;
        }
    }

    private void InitializeView()
    {
        _viewObj = Instantiate(_viewPrefab.gameObject, _canvas.transform);
        _view = _viewObj.GetComponent<ChoiceTextEventView>();
        if (_view == null)
        {
            Debug.LogError("ChoiceTextEventViewがアタッチされていません。");
            return;
        }
        _view.Initialize(_choices, _message);

        // Bind
        _view.OnFinish
            .Subscribe(_ =>
            {
                _isFinish = true;
            })
            .AddTo(_disposable);
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

    void OnDestroy()
    {
        _disposable?.Dispose();
    }
}
