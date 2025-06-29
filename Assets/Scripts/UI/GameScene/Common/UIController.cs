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
        private GameObject _inventoryUI;
        private GameObject _menuUI;

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

        public UIState(bool inventoryEnabled, bool menuEnabled, GameObject inventory, GameObject menu)
        {
            _inventoryEnabled = inventoryEnabled;
            _menuEnabled = menuEnabled;
            _inventoryUI = inventory;
            _menuUI = menu;
        }
    }

    [Header("UI Controller")]
    [SerializeField] private InventoryController _inventoryControllerPrefab;
    [SerializeField] private MenuController _menuControllerPrefab;
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
        _uiState = new UIState(false, false, null, null);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInventoryUI();
        UpdateMenuUI();
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
        if (Input.GetKeyDown(KeyCode.Escape) && !_uiState.InventoryEnabled)
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
