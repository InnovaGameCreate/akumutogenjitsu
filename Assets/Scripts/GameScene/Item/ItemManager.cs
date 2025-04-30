using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    /// <summary>  
    /// アイテムを所持しているかの状態  
    /// </summary>  
    private Dictionary<eItem, bool> _ownedItems = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created  
    void Start()
    {
        foreach (eItem item in System.Enum.GetValues(typeof(eItem)))
        {
            _ownedItems[item] = false;
        }
    }

    /// <summary>
    /// アイテムを所持しているかの状態を取得する
    /// </summary>
    /// <param name="item"> アイテムの種類 </param>
    /// <returns> 所持しているか </returns>
    public bool GetIsItemOwned(eItem item)
    {
        if (_ownedItems.TryGetValue(item , out bool isOwned))
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
        if (_ownedItems.ContainsKey(item))
        {
            _ownedItems[item] = isOwned;
        }
        else
        {
            Debug.LogError($"アイテム: {item} は存在しません。");
        }
    }
}
