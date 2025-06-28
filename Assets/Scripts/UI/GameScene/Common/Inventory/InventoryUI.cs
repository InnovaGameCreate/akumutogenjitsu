using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// インベントリUIの管理クラス
/// 9つのアイテムスロットとアイテム詳細表示を管理する
/// </summary>
public class InventoryUI : MonoBehaviour
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

    [Header("設定値")]
    [SerializeField] private int _selectedSlotIndex = 0; // 選択されているスロット

    private int _currentSelectedSolotIndex;

    private List<ItemData> _ownedItemDatas = new();

    /// <summary>
    /// アイテムのIndex
    /// </summary>
    public int SelectItemIndex
    {
        get => _selectedSlotIndex;
        set
        {
            if (value == _currentSelectedSolotIndex) return;

            _currentSelectedSolotIndex = value;
            _selectedSlotIndex = value;
            UpdateInventory();
            UpdateDescription(value);
        }
    }

    /// <summary>
    /// 所持しているItemData
    /// </summary>
    public List<ItemData> OwnedItemData
    {
        get => _ownedItemDatas;
        set
        {
            if (value == _ownedItemDatas) return;

            _ownedItemDatas = value;
            UpdateInventory();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        SelectItemIndex = _selectedSlotIndex;
    }

    private void UpdateInventory()
    {
        for (int i = 0; i < _slotBackgrounds.Length; i++)
        {
            UpdateIcon(i);
            UpdateSlotBackground(i);
        }
    }

    private void UpdateIcon(int index)
    {
        if (index < _ownedItemDatas.Count)
        {
            _itemIcons[index].sprite = _ownedItemDatas[index].Icon;
            _itemIcons[index].color = new Color(255, 255, 255, 1.0f);
        }
        else
        {
            _itemIcons[index].sprite = null;
            _itemIcons[index].color = new Color(255, 255, 255, 0);
        }
    }

    private void UpdateSlotBackground(int index)
    {
        _slotBackgrounds[index].sprite = (index == _selectedSlotIndex) ? _selectedSlotSprite : _notSelectedSlotSprite;
    }

    private void UpdateDescription(int index)
    {
        if (index > _ownedItemDatas.Count - 1)
        {
            _itemNameText.text = "";
            _itemDescriptionText.text = "";
            _itemDetailIcon.sprite = null;
            _itemDetailIcon.color = new Color(255, 255, 255, 0);
            return;
        }
        _itemNameText.text = _ownedItemDatas[index].Name;
        _itemDescriptionText.text = _ownedItemDatas[index].Description;
        _itemDetailIcon.sprite = _ownedItemDatas[index].Icon;
        _itemDetailIcon.color = new Color(255, 255, 255, 1.0f);
    }
}
