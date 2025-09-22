using System.Collections.Generic;
using R3;
using TMPro;
using UnityEngine;

public class ChoiceTextEventView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private GameObject _choicesBaseObj;

    [Header("Choice")]
    [SerializeField] private GameObject _choicePrefab;

    [SerializeField] private int _marginHeight = 100;
    [SerializeField] private int _centerHeight = 310;

    // Observable
    private readonly Subject<Unit> _moveToDown = new();
    public Observable<Unit> OnMoveToDown => _moveToDown;
    private readonly Subject<Unit> _moveToUp = new();
    public Observable<Unit> OnMoveToUp => _moveToUp;
    private readonly Subject<Unit> _select = new();
    public Observable<Unit> OnSelect => _select;

    // ChoiceTextEventでバインド
    private readonly Subject<Unit> _finish = new();
    public Observable<Unit> OnFinish => _finish;

    private List<GameObject> _choiceObjs = new();

    private ChoiceTextEventPresenter _presenter;

    // InputSystemコールバック参照（解除用）
    private System.Action<UnityEngine.InputSystem.InputAction.CallbackContext> _moveUpCallback;
    private System.Action<UnityEngine.InputSystem.InputAction.CallbackContext> _moveDownCallback;
    private System.Action<UnityEngine.InputSystem.InputAction.CallbackContext> _selectCallback;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_choicesBaseObj == null)
        {
            Debug.LogError("_choicesBaseObjが存在していません。");
            return;
        }

        BindToInput();
    }

    public void Initialize(List<Choice> choices, string message)
    {
        _presenter = new ChoiceTextEventPresenter(this, choices, message);
    }

    private void BindToInput()
    {
        // InputSystemの有効性チェック
        if (PlayerInput.Instance?.Input?.ChoiceTextEvent == null)
        {
            Debug.LogWarning("PlayerInput.Instance.Input.ChoiceTextEventが利用できません。");
            return;
        }

        // コールバック関数を定義（Subject破棄チェック付き）
        _moveUpCallback = ctx =>
        {
            if (ctx.ReadValueAsButton() && !_moveToUp.IsDisposed)
            {
                _moveToUp.OnNext(Unit.Default);
            }
        };

        _moveDownCallback = ctx =>
        {
            if (ctx.ReadValueAsButton() && !_moveToDown.IsDisposed)
            {
                _moveToDown.OnNext(Unit.Default);
            }
        };

        _selectCallback = ctx =>
        {
            if (ctx.ReadValueAsButton() && !_select.IsDisposed)
            {
                _select.OnNext(Unit.Default);
            }
        };

        // コールバックを登録
        PlayerInput.Instance.Input.ChoiceTextEvent.MoveToUp.performed += _moveUpCallback;
        PlayerInput.Instance.Input.ChoiceTextEvent.MoveToDown.performed += _moveDownCallback;
        PlayerInput.Instance.Input.ChoiceTextEvent.Select.performed += _selectCallback;
    }

    /// <summary>
    /// InputSystemコールバックの解除
    /// </summary>
    private void UnbindFromInput()
    {
        if (PlayerInput.Instance?.Input?.ChoiceTextEvent != null)
        {
            if (_moveUpCallback != null)
                PlayerInput.Instance.Input.ChoiceTextEvent.MoveToUp.performed -= _moveUpCallback;

            if (_moveDownCallback != null)
                PlayerInput.Instance.Input.ChoiceTextEvent.MoveToDown.performed -= _moveDownCallback;

            if (_selectCallback != null)
                PlayerInput.Instance.Input.ChoiceTextEvent.Select.performed -= _selectCallback;
        }

        // 参照をクリア
        _moveUpCallback = null;
        _moveDownCallback = null;
        _selectCallback = null;
    }

    /// <summary>
    /// 選択肢を追加する
    /// </summary>
    /// <param name="choice"></param>
    public void AddChoice(Choice choice)
    {
        GameObject obj = Instantiate(_choicePrefab, _choicesBaseObj.transform);
        ChoiceView view = obj.GetComponent<ChoiceView>();
        if (view == null)
        {
            Debug.LogError("ChoiceEventがアタッチされていません。");
        }
        view.SetChoice(choice);

        _choiceObjs.Add(obj);

        SetChoicesPosition();
    }

    private void SetChoicesPosition()
    {
        int numOfChoices = _choiceObjs.Count;
        int halfNum = numOfChoices / 2;
        if (numOfChoices % 2 == 0)
        {
            // 偶数個の時
            for (int i = 0; i < numOfChoices; ++i)
            {
                // 真ん中の2つ
                if (i == halfNum - 1)
                {
                    _choiceObjs[i].transform.position = new Vector3(640, _centerHeight + _marginHeight / 2, 0);
                    continue;
                }
                if (i == halfNum)
                {
                    _choiceObjs[i].transform.position = new Vector3(640, _centerHeight - _marginHeight / 2, 0);
                    continue;
                }

                if (i < halfNum)
                {
                    _choiceObjs[i].transform.position = new Vector3(640, _centerHeight + _marginHeight * (halfNum - 1 - i) + (_marginHeight / 2), 0);
                }
                else
                {
                    _choiceObjs[i].transform.position = new Vector3(640, _centerHeight - _marginHeight * (i - halfNum) - (_marginHeight / 2), 0);
                }
            }
        }
        else
        {
            // 奇数個の時
            for (int i = 0; i < numOfChoices; ++i)
            {
                // 真ん中
                if (i == halfNum)
                {
                    _choiceObjs[i].transform.position = new Vector3(640, _centerHeight + _marginHeight / 2, 0);
                }
                else if (i < halfNum)
                {
                    _choiceObjs[i].transform.position = new Vector3(640, _centerHeight + _marginHeight / 2 + _marginHeight * (halfNum - i), 0);
                }
                else
                {
                    _choiceObjs[i].transform.position = new Vector3(640, _centerHeight - _marginHeight / 2 - _marginHeight * (i - halfNum - 1), 0);
                }
            }
        }
    }

    /// <summary>
    /// 全ての選択肢を設定する
    /// </summary>
    /// <param name="choices"></param>
    public void SetChoices(List<Choice> choices)
    {
        // すでに要素が存在している場合は全て削除
        if (_choiceObjs.Count != 0)
        {
            foreach (var obj in _choiceObjs)
            {
                if (obj != null) Destroy(obj);
            }
            _choiceObjs.Clear();
        }
        foreach (var choice in choices)
        {
            AddChoice(choice);
        }
    }

    /// <summary>
    /// 選択されている選択肢を設定する
    /// </summary>
    /// <param name="index"></param>
    public void SetSelectedIndex(int index)
    {
        for (int i = 0; i < _choiceObjs.Count; ++i)
        {
            GameObject obj = _choiceObjs[i];
            if (obj == null) continue;

            ChoiceView view = obj.GetComponent<ChoiceView>();
            if (view == null)
            {
                Debug.LogError("ChoiceViewがアタッチされていません。");
                return;
            }

            view.SetIsSelected(i == index);
        }
    }

    /// <summary>
    /// メッセージを設定する
    /// </summary>
    /// <param name="message"></param>
    public void SetMessage(string message)
    {
        if (_messageText != null)
        {
            _messageText.text = message;
        }
    }

    public void FinishSelect()
    {
        // 重要：InputActionを先に無効化
        UnbindFromInput();

        // Subjectの通知
        if (!_finish.IsDisposed)
        {
            _finish.OnNext(Unit.Default);
        }
    }

    void OnDisable()
    {
        // GameObjectが無効化される際にInputコールバックを解除
        UnbindFromInput();
    }

    void OnDestroy()
    {
        // InputSystemコールバックを最優先で解除
        UnbindFromInput();

        // Subjectを破棄
        _moveToUp?.Dispose();
        _moveToDown?.Dispose();
        _select?.Dispose();
        _finish?.Dispose();

        // Presenterの参照をクリア
        _presenter = null;
    }
}
