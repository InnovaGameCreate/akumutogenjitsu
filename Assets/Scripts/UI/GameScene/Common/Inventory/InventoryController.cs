using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryUI _view;
    private int _slotNum = 9;
    [SerializeField] private int _slotWidth = 3;
    [SerializeField] private int _slotHeight = 3;
    private ItemManager _itemManager;

    private List<ItemData> _currentOwnedItemData;

    private int _selectedIndex = 0;

    public List<ItemData> OwnedItemDatas
    {
        get => _currentOwnedItemData;
        set
        {
            if (value == _currentOwnedItemData) return;

            _currentOwnedItemData = value;
            _view.OwnedItemData = value;
        }
    }

    public int SelectedSlotIndex
    {
        get => _selectedIndex;
        set
        {
            if (value == _selectedIndex) return;
            if (value < 0 || value > 8)
            {
                Debug.LogError($"SlotIndexに不正な値が渡されました。({value})");
                return;
            }

            _selectedIndex = value;
            _view.SelectItemIndex = _selectedIndex;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _itemManager = GameObject.FindGameObjectWithTag("ItemMgr").GetComponent<ItemManager>();
        if (_itemManager == null)
        {
            Debug.LogError("ItemManagerが見つかりません。");
        }
        _slotNum = _slotHeight * _slotWidth;
    }

    // Update is called once per frame
    void Update()
    {
        OwnedItemDatas = _itemManager.OwnedItemDatas;

        SelectedSlotIndex = GetSelectedSlotIndex();
    }

    private int GetSelectedSlotIndex()
    {
        int index = _selectedIndex;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (index <= 0 || index % 3 == 0) return index;
            index--;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (index > _slotNum || index % 3 == 2) return index;
            index++;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (index - 3 < 0) return index;
            index -= 3;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (index + 3 >= _slotNum) return index;
            index += 3;
        }
        return index;
    }
}
