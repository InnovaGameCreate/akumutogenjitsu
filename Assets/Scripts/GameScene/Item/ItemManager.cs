using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    /// <summary>  
    /// �A�C�e�����������Ă��邩�̏��  
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
    /// �A�C�e�����������Ă��邩�̏�Ԃ��擾����
    /// </summary>
    /// <param name="item"> �A�C�e���̎�� </param>
    /// <returns> �������Ă��邩 </returns>
    public bool GetIsItemOwned(eItem item)
    {
        if (_ownedItems.TryGetValue(item , out bool isOwned))
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
        if (_ownedItems.ContainsKey(item))
        {
            _ownedItems[item] = isOwned;
        }
        else
        {
            Debug.LogError($"�A�C�e��: {item} �͑��݂��܂���B");
        }
    }
}
