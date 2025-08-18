using UnityEngine;

public class UIController : MonoBehaviour
{
    /// <summary>
    /// UIの表示状態を管理する構造体
    /// </summary>
    private struct UIState
    {
        private bool _inventoryEnabled;
        private bool _menuEnabled;
        private bool _saveMenuEnabled;
        private GameObject _inventoryUI;
        private GameObject _menuUI;
        private GameObject _saveMenuUI;

        /// <summary>
        /// インベントリが表示されているか
        /// </summary>
        public bool InventoryEnabled
        {
            get { return _inventoryEnabled; }
            set { _inventoryEnabled = value; }
        }
        /// <summary>
        /// メニューが表示されているか
        /// </summary>
        public bool MenuEnabled
        {
            get { return _menuEnabled; }
            set { _menuEnabled = value; }
        }

        /// <summary>
        /// セーブメニューが表示されているか
        /// </summary>
        public bool SaveMenuEnabled
        { 
            set { _saveMenuEnabled = value; }
            get { return _saveMenuEnabled; }
        }

        /// <summary>
        /// インベントリのUI
        /// </summary>
        public GameObject InventoryUI
        {
            get { return _inventoryUI; }
            set { _inventoryUI = value; }
        }
        /// <summary>
        /// メニューのUI
        /// </summary>
        public GameObject MenuUI
        {
            get { return _menuUI; }
            set { _menuUI = value; }
        }

        /// <summary>
        /// セーブメニューのUI
        /// </summary>
        public GameObject SaveMenuUI
        {
            get { return _saveMenuUI; }
            set { _saveMenuUI = value; }
        }

        public UIState(bool inventoryEnabled, bool menuEnabled, bool saveMenuEnabled, GameObject inventory, GameObject menu, GameObject saveMenu)
        {
            _inventoryEnabled = inventoryEnabled;
            _menuEnabled = menuEnabled;
            _saveMenuEnabled = saveMenuEnabled;
            _inventoryUI = inventory;
            _menuUI = menu;
            _saveMenuUI = saveMenu;
            SaveMenuUI = saveMenu;
        }
    }

    [Header("UI Controller")]
    [SerializeField] private InventoryController _inventoryControllerPrefab;
    [SerializeField] private MenuController _menuControllerPrefab;
    [SerializeField] private SaveMenuView _saveMenuPrefab;
    [Header("Canvas")]
    [SerializeField] private Canvas _canvas;
    [Header("UnitMove(アサインしなくてもいい)")]
    [SerializeField] private UnitMove _playerMove;

    private UIState _uiState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_inventoryControllerPrefab == null)
        {
            Debug.LogError("InventoryControllerがアサインされていません。");
            return;
        }
        if (_menuControllerPrefab == null)
        {
            Debug.LogError("MenuControllerがアサインされていません。");
            return;
        }
        if (_playerMove == null)
        {
            _playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<UnitMove>();
            if (_playerMove == null)
            {
                Debug.LogError("UnitMoveが見つかりませんでした。");
                return;
            }
            Debug.Log("Playerがアサインされていませんが、Playerが見つかりました。");
        }
        if (_canvas == null)
        {
            Debug.LogError("Canvasがアサインされていません。");
            return;
        }
        
        // UIStateを初期化
        _uiState = new UIState(false, false, false, null, null, null);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInventoryUI();
        UpdateMenuUI();
        UpdateSaveMenuUI();
    }

    private void UpdateInventoryUI()
    {
        if (Input.GetKeyDown(KeyCode.X) && !_uiState.MenuEnabled)
        {
            if (!_uiState.InventoryEnabled)
            {
                GameObject inventoryUI = CreateUI(_inventoryControllerPrefab.gameObject);
                _uiState.InventoryEnabled = true;
                _uiState.InventoryUI = inventoryUI;
            }
            else
            {
                if (_uiState.InventoryUI != null)
                {
                    Destroy(_uiState.InventoryUI);
                    _uiState.InventoryUI = null;
                    _uiState.InventoryEnabled = false;
                    _playerMove.IsEnabled = true;
                }
            }
        }
    }

    private void UpdateMenuUI()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_uiState.InventoryEnabled && !_uiState.SaveMenuEnabled)
        {
            if (!_uiState.MenuEnabled)
            {
                GameObject menuUI = CreateUI(_menuControllerPrefab.gameObject);
                _uiState.MenuEnabled = true;
                _uiState.MenuUI = menuUI;
            }
            else
            {
                if (_uiState.MenuUI != null)
                {
                    Destroy(_uiState.MenuUI);
                    _uiState.MenuUI = null;
                    _uiState.MenuEnabled = false;
                    _playerMove.IsEnabled = true;
                }
            }
        }
    }

    private void UpdateSaveMenuUI()
    {
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)) && _uiState.MenuEnabled && _uiState.MenuUI.GetComponent<MenuUI>().SelectedIndex == 0)
        {
            if (!_uiState.SaveMenuEnabled)
            {
                GameObject saveMenuUI = CreateUI(_saveMenuPrefab.gameObject);
                _uiState.SaveMenuEnabled = true;
                _uiState.SaveMenuUI = saveMenuUI;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && _uiState.SaveMenuEnabled)
        {
            if (_uiState.SaveMenuUI != null)
            {
                Destroy(_uiState.SaveMenuUI);
                _uiState.SaveMenuUI = null;
                _uiState.SaveMenuEnabled = false;
            }
        }
    }

    private GameObject CreateUI(GameObject uiPrefab)
    {
        GameObject uiObj = Instantiate(uiPrefab);
        uiObj.transform.SetParent(_canvas.gameObject.transform, false);

        // 位置を(0, 0, 0)に設定
        uiObj.transform.localPosition = Vector3.zero;
        uiObj.transform.localScale = Vector3.one;

        // Playerの動きを止める
        _playerMove.IsEnabled = false;

        return uiObj;
    }
}
