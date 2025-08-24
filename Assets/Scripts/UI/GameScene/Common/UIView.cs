using R3;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _baseUIPrefab;
    [SerializeField] private GameObject _inventoryUIPrefab;
    [SerializeField] private GameObject _menuUIPrefab;
    [SerializeField] private GameObject _saveMenuUIPrefab;

    [Header("UIを表示するCanvas")]
    [SerializeField] private Canvas _canvas;

    // 生成したUI
    private GameObject _baseUIObj;
    private GameObject _inventoryUIObj;
    private GameObject _menuUIObj;
    private GameObject _saveMenuUIObj;

    private readonly Subject<Unit> _onShowBase = new();
    public Observable<Unit> OnShowBase => _onShowBase;

    private readonly Subject<Unit> _onShowInventory = new();
    public Observable<Unit> OnShowInventory => _onShowInventory;

    private readonly Subject<Unit> _onShowMenu = new();
    public Observable<Unit> OnShowMenu => _onShowMenu;

    private readonly Subject<Unit> _onShowSaveMenu = new();
    public Observable<Unit> OnShowSaveMenu => _onShowSaveMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform parent = _canvas != null ? _canvas.transform : transform;

        _baseUIObj = Instantiate(_baseUIPrefab, parent);
        _inventoryUIObj = Instantiate(_inventoryUIPrefab, parent);
        _menuUIObj = Instantiate(_menuUIPrefab, parent);
        _saveMenuUIObj = Instantiate(_saveMenuUIPrefab, parent);

        _baseUIObj.SetActive(true);
        _inventoryUIObj.SetActive(false);
        _menuUIObj.SetActive(false);
        _saveMenuUIObj.SetActive(false);
    }

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

    public void ShowSaveMenu(bool active)
    {
        _saveMenuUIObj?.SetActive(active);
    }

    public void OnBaseInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _onShowBase?.OnNext(Unit.Default);
        }
    }

    public void OnInventoryInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _onShowInventory?.OnNext(Unit.Default);
        }
    }

    public void OnMenuInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _onShowMenu?.OnNext(Unit.Default);
        }
    }

    public void OnSaveMenuInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _onShowSaveMenu?.OnNext(Unit.Default);
        }
    }

    void OnDestroy()
    {
        _onShowBase?.Dispose();
        _onShowInventory?.Dispose();
        _onShowMenu?.Dispose();
        _onShowSaveMenu?.Dispose();
    }
}
