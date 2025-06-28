using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private List<ItemData> _itemDatas;
    [SerializeField] private InventoryUI _inventoryUIPrefab;

    [Header("Player��UnitMove")]
    [SerializeField] private UnitMove _playerMove;

    private bool _isDisplayingInventory = false;

    private GameObject _inventoryUIObj;

    /// <summary>  
    /// �A�C�e�����������Ă��邩�̏��  
    /// </summary>  
    private Dictionary<eItem, bool> _itemOwned = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created  
    void Start()
    {
        foreach (eItem item in System.Enum.GetValues(typeof(eItem)))
        {
            _itemOwned[item] = false;
        }
        _itemOwned[eItem.MedicineBlue] = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_isDisplayingInventory)
            {
                Destroy(_inventoryUIObj);
                _isDisplayingInventory = false;
                _playerMove.IsEnabled = true;
            }
            else
            {
                Canvas canvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
                if (canvas == null)
                {
                    Debug.LogError("Canvas��������Ȃ��ł��B");
                    return;
                }
                InventoryUI inventoryUI = Instantiate(_inventoryUIPrefab);
                inventoryUI.transform.SetParent(canvas.transform, false);
                _inventoryUIObj = inventoryUI.gameObject;
                _isDisplayingInventory = true;
                _playerMove.IsEnabled = false;
            }
        }
    }

    /// <summary>
    /// �A�C�e�����������Ă��邩�̏�Ԃ��擾����
    /// </summary>
    /// <param name="item"> �A�C�e���̎�� </param>
    /// <returns> �������Ă��邩 </returns>
    public bool GetIsItemOwned(eItem item)
    {
        if (_itemOwned.TryGetValue(item, out bool isOwned))
        {
            return isOwned;
        }
        else
        {
            Debug.LogError($"�A�C�e��: {item} �͑��݂��܂���B");
            return false;
        }
    }

    /// <summary>
    /// �A�C�e�����������Ă��邩�̏�Ԃ�ݒ肷��
    /// </summary>
    /// <param name="item"> �A�C�e���̎�� </param>
    /// <param name="isOwned"> �ێ����邩 </param>
    public void SetIsItemOwned(eItem item, bool isOwned)
    {
        if (_itemOwned.ContainsKey(item))
        {
            _itemOwned[item] = isOwned;
        }
        else
        {
            Debug.LogError($"�A�C�e��: {item} �͑��݂��܂���B");
        }
    }

    /// <summary>
    /// eItem����ItemData���擾����
    /// </summary>
    /// <param name="itemType"> �A�C�e���̎�� </param>
    /// <returns> ItemData�i������Ȃ��ꍇ��null�j </returns>
    public ItemData GetItemData(eItem itemType)
    {
        return _itemDatas.Find(data => data.ItemType == itemType);
    }

    /// <summary>
    /// �������Ă���ItemData
    /// </summary>
    public List<ItemData> OwnedItemDatas
    {
        get
        {
            List<ItemData> itemDatas = new();
            foreach (ItemData item in _itemDatas)
            {
                if (_itemOwned[item.ItemType])
                {
                    itemDatas.Add(item);
                }
            }
            return itemDatas;
        }
    }
}
