using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private List<ItemData> _itemDatas;

    /// <summary>  
    /// アイテムを所持しているかの状態  
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

    private void Update()
    {
    }

    /// <summary>
    /// アイテムを所持しているかの状態を取得する
    /// </summary>
    /// <param name="item"> アイテムの種類 </param>
    /// <returns> 所持しているか </returns>
    public bool GetIsItemOwned(eItem item)
    {
        if (_itemOwned.TryGetValue(item, out bool isOwned))
        {
            return isOwned;
        }
        else
        {
            Debug.LogError($"アイテム: {item} は存在しません。");
            return false;
        }
    }

    /// <summary>
    /// アイテムを所持しているかの状態を設定する
    /// </summary>
    /// <param name="item"> アイテムの種類 </param>
    /// <param name="isOwned"> 保持するか </param>
    public void SetIsItemOwned(eItem item, bool isOwned)
    {
        if (_itemOwned.ContainsKey(item))
        {
            _itemOwned[item] = isOwned;
        }
        else
        {
            Debug.LogError($"アイテム: {item} は存在しません。");
        }
    }

    /// <summary>
    /// eItemからItemDataを取得する
    /// </summary>
    /// <param name="itemType"> アイテムの種類 </param>
    /// <returns> ItemData（見つからない場合はnull） </returns>
    public ItemData GetItemData(eItem itemType)
    {
        return _itemDatas.Find(data => data.ItemType == itemType);
    }

    /// <summary>
    /// 所持しているItemData
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
