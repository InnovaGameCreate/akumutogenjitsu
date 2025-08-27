using R3;
using UnityEngine;

public class UIView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _baseUIPrefab;
    [SerializeField] private GameObject _inventoryUIPrefab;
    [SerializeField] private GameObject _menuUIPrefab;

    [Header("UIを表示するCanvas")]
    [SerializeField] private Canvas _canvas;

    // 生成したUI
    private GameObject _baseUIObj;
    private GameObject _inventoryUIObj;
    private GameObject _menuUIObj;

    private readonly Subject<Unit> _baseInput = new();
    public Observable<Unit> BaseInput => _baseInput;

    private readonly Subject<Unit> _inventoryInput = new();
    public Observable<Unit> InventoryInput => _inventoryInput;

    private readonly Subject<Unit> _menuInput = new();
    public Observable<Unit> MenuInput => _menuInput;

    void Awake()
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform parent = _canvas != null ? _canvas.transform : transform;

        _baseUIObj = Instantiate(_baseUIPrefab, parent);
        _inventoryUIObj = Instantiate(_inventoryUIPrefab, parent);
        _menuUIObj = Instantiate(_menuUIPrefab, parent);

        _baseUIObj.SetActive(true);
        _inventoryUIObj.SetActive(false);
        _menuUIObj.SetActive(false);

        BindToInput();
    }

    public void BindToInput()
    {
        PlayerInput.Instance.Input.Base.OpenMenu.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
            {
                OnMenuInput();
            }
        };

        PlayerInput.Instance.Input.Base.OpenInventory.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
            {
                OnInventoryInput();
            }
        };

        PlayerInput.Instance.Input.Base.Enable();
    }

    // MARK: Show
    
    public void ShowBase(bool active)
    {
        _baseUIObj?.SetActive(active);
    }

    public void ShowInventory(bool active)
    {
        _inventoryUIObj?.SetActive(active);
    }

    public void ShowMenu(bool active)
    {
        _menuUIObj?.SetActive(active);
    }

    // MARK: Input

    public void OnBaseInput()
    {
        _baseInput?.OnNext(Unit.Default);
    }

    public void OnInventoryInput()
    {
        _inventoryInput?.OnNext(Unit.Default);
    }

    public void OnMenuInput()
    {
        _menuInput?.OnNext(Unit.Default);
    }

    // MARK: ActionMap
    public void ActionMapToBase(bool active)
    {
        if (active)
        {
            PlayerInput.Instance.Input.Base.Enable();
        }
        else
        {
            PlayerInput.Instance.Input.Base.Disable();
        }
    }

    public void ActionMapToMenu(bool active)
    {
        if (active)
        {
            PlayerInput.Instance.Input.Menu.Enable();
        }
        else
        {
            PlayerInput.Instance.Input.Menu.Disable();
        }
    }

    public void ActionMapToInventory(bool active)
    {
        if (active)
        {
            PlayerInput.Instance.Input.Inventory.Enable();
        }
        else
        {
            PlayerInput.Instance.Input.Inventory.Disable();
        }
    }

    void OnDestroy()
    {
        _baseInput?.Dispose();
        _inventoryInput?.Dispose();
        _menuInput?.Dispose();
    }
}
