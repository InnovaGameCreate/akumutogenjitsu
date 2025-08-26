using R3;
using UnityEngine;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
    [Header("ボタン")]
    [SerializeField] private Image _saveButton;
    [SerializeField] private Image _titleButton;

    [Header("スプライト")]
    [SerializeField] private Sprite _selectedSaveSprite;
    [SerializeField] private Sprite _notSelectedSaveSprite;
    [SerializeField] private Sprite _selectedTitleSprite;
    [SerializeField] private Sprite _notSelectedTitleSprite;

    [SerializeField] private GameObject _saveMenuPrefab;
    private GameObject _saveMenuObj;

    private GameObject _canvas;

    private readonly Subject<Unit> _moveLeft = new();
    public Observable<Unit> MoveLeft => _moveLeft;

    private readonly Subject<Unit> _moveRight = new();
    public Observable<Unit> MoveRight => _moveRight;

    private readonly Subject<Unit> _select = new();
    public Observable<Unit> Select => _select;

    private readonly Subject<Unit> _close = new();
    public Observable<Unit> Close => _close;

    private PlayerOperation _playerOperation;

    void Awake()
    {
        _playerOperation = new PlayerOperation();
    }

    void Start()
    {
        _canvas = GameObject.FindWithTag("UICanvas");
        Transform parent = _canvas != null ? _canvas.transform : transform;

        _saveMenuObj = Instantiate(_saveMenuPrefab, parent);
        _saveMenuObj.SetActive(false);

        BindToInput();
        SelectItem(0);
    }

    void OnEnable()
    {
        if (_playerOperation != null)
        {
            ActionMapToMenu(true);
        }
    }

    void OnDisable()
    {
        if (_playerOperation != null)
        {
            ActionMapToMenu(false);
        }
    }

    public void BindToInput()
    {
        _playerOperation.Menu.MoveToLeft.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
            {
                OnMoveLeftInput();
            }
        };

        _playerOperation.Menu.MoveToRight.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
            {
                OnMoveRightInput();
            }
        };

        _playerOperation.Menu.Select.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
            {
                OnSelectInput();
            }
        };

        _playerOperation.Menu.Close.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
            {
                OnCloseInput();
            }
        };
    }

    // MARK: Show
    public void SelectItem(int index)
    {
        switch (index)
        {
            case 0:
                _saveButton.sprite = _selectedSaveSprite;
                _titleButton.sprite = _notSelectedTitleSprite;
                break;
            case 1:
                _saveButton.sprite = _notSelectedSaveSprite;
                _titleButton.sprite = _selectedTitleSprite;
                break;
            default:
                Debug.LogError("不明な選択肢が選択されています。");
                break;
        }
    }

    public void ShowSaveMenu(bool active)
    {
        _saveMenuObj.SetActive(active);
    }

    // MARK: Input
    public void OnMoveLeftInput()
    {
        _moveLeft?.OnNext(Unit.Default);
    }

    public void OnMoveRightInput()
    {
        _moveRight?.OnNext(Unit.Default);
    }

    public void OnSelectInput()
    {
        _select?.OnNext(Unit.Default);
    }

    public void OnCloseInput()
    {
        _close?.OnNext(Unit.Default);
    }

    // MARK: ActionMap
    public void ActionMapToMenu(bool active)
    {
        if (active)
        {
            _playerOperation.Menu.Enable();
        }
        else
        {
            _playerOperation.Menu.Disable();
        }
    }

    public void ActionMapToSave(bool active)
    {
        if (active)
        {
            _playerOperation.Save.Enable();
        }
        else
        {
            _playerOperation.Save.Disable();
        }
    }

    void OnDestroy()
    {
        _playerOperation?.Dispose();
        _moveLeft?.Dispose();
        _moveRight?.Dispose();
        _select?.Dispose();
        _close?.Dispose();
    }
}
