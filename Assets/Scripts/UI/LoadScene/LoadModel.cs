using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

public struct SlotItem
{
    public string date;
    public string title;
}

public struct LoadModelOutput
{
    public ReadOnlyReactiveProperty<int> slotIndex;
    public ReadOnlyReactiveProperty<List<SlotItem>> slotItems;
}

public static class LoadConstants
{
    public const int maxSaveSlot = 3;
    public const string emptySlotText = "空のスロット";
}

class LoadModel
{
    private SaveManager _saveMgr;

    public ReadOnlyReactiveProperty<int> ActiveSlotIndex => _activeSlotIndex;
    private readonly ReactiveProperty<int> _activeSlotIndex = new(0);

    public ReadOnlyReactiveProperty<List<SlotItem>> SlotItems => _slotItems;
    private readonly ReactiveProperty<List<SlotItem>> _slotItems = new(new List<SlotItem>());

    public LoadModel(int defaultIndex)
    {
        _activeSlotIndex.Value = defaultIndex;
        Initialize();
    }

    public LoadModelOutput Bind()
    {
        return new LoadModelOutput
        {
            slotIndex = _activeSlotIndex,
            slotItems = _slotItems
        };
    }

    private void Initialize()
    {
        GameObject saveMgr = GameObject.FindWithTag("SaveMgr");
        if (saveMgr == null)
        {
            Debug.LogError("SaveMgrが存在しません。");
            return;
        }

        _saveMgr = saveMgr.GetComponent<SaveManager>();
        if (_saveMgr == null)
        {
            Debug.LogError("SaveManagerがコンポーネントされていません。");
            return;
        }

        LoadSlotItems();
    }

    /// <summary>
    /// セーブデータスロットを読み込む
    /// </summary>
    private void LoadSlotItems()
    {
        var newList = new List<SlotItem>();

        for (int i = 0; i < LoadConstants.maxSaveSlot; i++)
        {
            SaveData saveData = _saveMgr.GetSaveData(i);
            SlotItem slotItem = new SlotItem();

            try
            {
                if (saveData != null)
                {
                    Date savedDate = new Date(saveData.DateData.Month, saveData.DateData.Day);
                    string dateString = Date.Format(savedDate);
                    int diffDays = Date.DiffDate(savedDate, Date.FirstDate);
                    string location = SceneLocationManager.Instance.GetLocationDisplayNameFromSceneName(saveData.SystemData.CurrentSceneName);

                    slotItem.date = saveData.SystemData.SystemDate;
                    slotItem.title = $"{dateString}({(Date.IsEarlier(savedDate, Date.FirstDate) || Date.IsSameDate(savedDate, Date.FirstDate) ? (diffDays + 1) : (-diffDays - 1))}日目) - {location}";
                }
                else
                {
                    slotItem.date = "0000/00/00 00:00";
                    slotItem.title = LoadConstants.emptySlotText;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"スロット{i}の読み込みに失敗しました: {ex.Message}");
                slotItem.date = "0000/00/00 00:00";
                slotItem.title = "読み込みエラー";
            }

            newList.Add(slotItem);
        }

        _slotItems.Value = newList;
    }

    public void MoveUp()
    {
        _activeSlotIndex.Value = (_activeSlotIndex.Value - 1 + LoadConstants.maxSaveSlot) % LoadConstants.maxSaveSlot;
    }

    public void MoveDown()
    {
        _activeSlotIndex.Value = (_activeSlotIndex.Value + 1) % LoadConstants.maxSaveSlot;
    }

    /// <summary>
    /// 選択中のスロットのセーブデータを取得
    /// </summary>
    public SaveData GetCurrentSaveData()
    {
        if (_saveMgr == null)
        {
            Debug.LogError("SaveManagerが初期化されていません。");
            return null;
        }

        return _saveMgr.GetSaveData(_activeSlotIndex.Value);
    }

    /// <summary>
    /// 指定したスロットのセーブデータを取得
    /// </summary>
    public SaveData GetSaveData(int slotIndex)
    {
        if (_saveMgr == null)
        {
            Debug.LogError("SaveManagerが初期化されていません。");
            return null;
        }

        if (slotIndex < 0 || slotIndex >= LoadConstants.maxSaveSlot)
        {
            Debug.LogError($"不正なスロットインデックス: {slotIndex}");
            return null;
        }

        return _saveMgr.GetSaveData(slotIndex);
    }

    /// <summary>
    /// セーブデータスロットを再読み込み
    /// </summary>
    public void RefreshSlotItems()
    {
        LoadSlotItems();
    }

    public bool LoadSaveData()
    {
        return _saveMgr.LoadFromFile(_activeSlotIndex.Value);
    }
}
