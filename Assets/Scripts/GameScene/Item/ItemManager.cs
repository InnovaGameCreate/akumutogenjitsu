using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour, ISaveableManager<ItemSaveData>
{
    [SerializeField] private List<ItemData> _itemDatas;

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

    public ItemSaveData EncodeToSaveData()
    {
        ItemSaveData saveData = new ItemSaveData();
        foreach (var item in _itemOwned)
        {
            if (item.Value)
            {
                saveData.OwnedItems.Add(item.Key);
            }
        }
        return saveData;
    }

    public void LoadFromSaveData(ItemSaveData saveData)
    {
        foreach (var item in _itemOwned.Keys.ToList())
        {
            _itemOwned[item] = false;
        }
        
        foreach (eItem savedItem in saveData.OwnedItems)
        {
            if (_itemOwned.ContainsKey(savedItem))
            {
                _itemOwned[savedItem] = true;
            }
        }
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
