using System.Collections.Generic;
using R3;
using UnityEngine;

public class InventoryModel : MonoBehaviour
{
    private readonly ReactiveProperty<int> _selectedIndex = new(0);
    public ReadOnlyReactiveProperty<int> SelectedIndex => _selectedIndex;

    private readonly ReactiveProperty<List<ItemData>> _ownedItemDatas = new(new List<ItemData>());
    public ReadOnlyReactiveProperty<List<ItemData>> OwnedItemDatas => _ownedItemDatas;

    private readonly int _slotWidth = 3;
    private readonly int _totalSlots = 9;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _selectedIndex.Value = 0;
        UpdateItemData();
    }

    public void UpdateItemData()
    {
        _ownedItemDatas.Value = ItemManager.Instance.OwnedItemDatas;
    }

    public void MoveSelection(Vector2 direction)
    {
        int currentIndex = _selectedIndex.Value;
        int newIndex = currentIndex;

        if (direction.x < -0.5f) // Left
        {
            if (currentIndex > 0 && currentIndex % _slotWidth != 0)
            {
                newIndex = currentIndex - 1;
            }
        }
        else if (direction.x > 0.5f) // Right
        {
            if (currentIndex < _totalSlots - 1 && currentIndex % _slotWidth != _slotWidth - 1)
            {
                newIndex = currentIndex + 1;
            }
        }
        else if (direction.y > 0.5f) // Up
        {
            if (currentIndex - _slotWidth >= 0)
            {
                newIndex = currentIndex - _slotWidth;
            }
        }
        else if (direction.y < -0.5f) // Down
        {
            if (currentIndex + _slotWidth < _totalSlots)
            {
                newIndex = currentIndex + _slotWidth;
            }
        }

        if (newIndex != currentIndex)
        {
            _selectedIndex.Value = newIndex;
        }
    }

    public void SetSelectedIndex(int index)
    {
        if (index >= 0 && index < _totalSlots)
        {
            _selectedIndex.Value = index;
        }
    }

    void OnEnable()
    {
        Initialize();
    }
}
