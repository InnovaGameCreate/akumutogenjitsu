using System;
using System.Collections.Generic;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SaveSlotView
{
    [SerializeField] public Image SlotBg;
    [SerializeField] public TextMeshProUGUI Date;
    [SerializeField] public TextMeshProUGUI Title;
}

public class SaveMenuView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private List<SaveSlotView> _slots;

    [Header("Sprite")]
    [SerializeField] private Sprite _activeSlotBg;
    [SerializeField] private Sprite _inactiveSlotBg;

    public Observable<KeyCode> OnKeyPressed =>
        Observable.EveryUpdate()
            .Select(_ =>
            {
                if (Input.GetKeyDown(KeyCode.UpArrow)) return KeyCode.UpArrow;
                if (Input.GetKeyDown(KeyCode.DownArrow)) return KeyCode.DownArrow;
                if (Input.GetKeyDown(KeyCode.Return)) return KeyCode.Return;
                return KeyCode.None;
            })
            .Where(key => key != KeyCode.None);

    public void ChangeActiveSlot(int slot)
    {
        if (slot < 0 || slot >= _slots.Count)
        {
            Debug.LogError("スロットは0～2にしてください。");
            return;
        }

        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i]?.SlotBg != null)
                _slots[i].SlotBg.sprite = i == slot ? _activeSlotBg : _inactiveSlotBg;
        }
    }
}
