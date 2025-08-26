using System.Collections.Generic;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    [Header("インベントリスロット (9個)")]
    [SerializeField] private Image[] _slotBackgrounds = new Image[9];
    [SerializeField] private Image[] _itemIcons = new Image[9];

    [Header("アイテム説明表示")]
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private Image _itemDetailIcon;
    [SerializeField] private TextMeshProUGUI _itemDescriptionText;

    [Header("スロット背景スプライト")]
    [SerializeField] private Sprite _selectedSlotSprite;
    [SerializeField] private Sprite _notSelectedSlotSprite;

    private readonly Subject<Vector2> _move = new();
    public Observable<Vector2> Move => _move;

    private readonly Subject<Unit> _close = new();
    public Observable<Unit> Close => _close;

    private PlayerOperation _playerOperation;

    void Awake()
    {
        _playerOperation = new PlayerOperation();
    }

    void Start()
    {
        BindToInput();
    }

    void OnEnable()
    {
        if (_playerOperation != null)
        {
            ActionMapToInventory(true);
        }
    }

    void OnDisable()
    {
        if (_playerOperation != null)
        {
            ActionMapToInventory(false);
        }
    }

    public void BindToInput()
    {
        _playerOperation.Inventory.Move.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
            {
                // 矢印キーの方向判定
                Vector2 direction = Vector2.zero;
                
                if (ctx.action.activeControl.name == "upArrow")
                    direction = Vector2.up;
                else if (ctx.action.activeControl.name == "downArrow")
                    direction = Vector2.down;
                else if (ctx.action.activeControl.name == "leftArrow")
                    direction = Vector2.left;
                else if (ctx.action.activeControl.name == "rightArrow")
                    direction = Vector2.right;
                
                OnMoveInput(direction);
            }
        };

        _playerOperation.Inventory.Close.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
            {
                OnCloseInput();
            }
        };
    }

    // MARK: Show
    public void UpdateInventory(List<ItemData> ownedItemDatas, int selectedIndex)
    {
        for (int i = 0; i < _slotBackgrounds.Length; i++)
        {
            UpdateIcon(i, ownedItemDatas);
            UpdateSlotBackground(i, selectedIndex);
        }
        UpdateDescription(selectedIndex, ownedItemDatas);
    }

    private void UpdateIcon(int index, List<ItemData> ownedItemDatas)
    {
        if (index < ownedItemDatas.Count)
        {
            _itemIcons[index].sprite = ownedItemDatas[index].Icon;
            _itemIcons[index].color = new Color(1, 1, 1, 1.0f);
        }
        else
        {
            _itemIcons[index].sprite = null;
            _itemIcons[index].color = Color.clear;
        }
    }

    private void UpdateSlotBackground(int index, int selectedIndex)
    {
        _slotBackgrounds[index].sprite = (index == selectedIndex) ? _selectedSlotSprite : _notSelectedSlotSprite;
    }

    private void UpdateDescription(int index, List<ItemData> ownedItemDatas)
    {
        if (index >= ownedItemDatas.Count || index < 0)
        {
            _itemNameText.text = "";
            _itemDescriptionText.text = "";
            _itemDetailIcon.sprite = null;
            _itemDetailIcon.color = Color.clear;
            return;
        }
        _itemNameText.text = ownedItemDatas[index].Name;
        _itemDescriptionText.text = ownedItemDatas[index].Description;
        _itemDetailIcon.sprite = ownedItemDatas[index].Icon;
        _itemDetailIcon.color = new Color(1, 1, 1, 1.0f);
    }

    public void ReturnToBase()
    {
        var uiView = GameObject.FindWithTag("UICanvas")?.GetComponent<UIView>();
        if (uiView != null)
        {
            uiView.OnInventoryInput();
        }
    }

    // MARK: Input
    public void OnMoveInput(Vector2 direction)
    {
        _move?.OnNext(direction);
    }

    public void OnCloseInput()
    {
        _close?.OnNext(Unit.Default);
    }

    // MARK: ActionMap
    public void ActionMapToInventory(bool active)
    {
        if (active)
        {
            _playerOperation.Inventory.Enable();
        }
        else
        {
            _playerOperation.Inventory.Disable();
        }
    }

    void OnDestroy()
    {
        _playerOperation?.Dispose();
        _move?.Dispose();
        _close?.Dispose();
    }
}
