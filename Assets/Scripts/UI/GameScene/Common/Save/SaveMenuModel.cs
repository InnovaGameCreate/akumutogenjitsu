using System.Collections.Generic;
using System.IO;
using R3;
using UnityEngine;

public struct SaveList
{
    public string Title;
    public string Date;
}

public static class SaveConstants
{
    public const int MAX_SAVE_SLOTS = 3;
    public const string EMPTY_SLOT_TEXT = "空のスロット";
}

public class SaveMenuModel : MonoBehaviour
{
    private SaveManager _saveMgr;
    private FileSystemWatcher _fileWatcher;

    public ReadOnlyReactiveProperty<int> ActiveSlotIndex => _activeSlotIndex;
    private readonly ReactiveProperty<int> _activeSlotIndex = new(0);

    public ReadOnlyReactiveProperty<List<SaveList>> SaveTitleList => _saveTitleList;
    private readonly ReactiveProperty<List<SaveList>> _saveTitleList = new();

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        if (_shouldUpdateUI)
        {
            _shouldUpdateUI = false;
            UpdateSaveTitleList();
        }
    }

    void OnDestroy()
    {
        // ファイル監視を停止
        if (_fileWatcher != null)
        {
            _fileWatcher.EnableRaisingEvents = false;
            _fileWatcher.Dispose();
            _fileWatcher = null;
        }
    }

    public void Initialize()
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
            Debug.LogError("SaveManagerがコンポーネントされていないです。");
            return;
        }

        _activeSlotIndex.Value = 0;

        // ファイル監視を開始
        SetupFileWatcher();
    }

    /// <summary>
    /// セーブファイルの監視を設定
    /// </summary>
    private void SetupFileWatcher()
    {
        try
        {
            string saveDirectory = Path.Combine(Application.persistentDataPath, "Saves");

            // ディレクトリが存在しない場合は作成
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            _fileWatcher = new FileSystemWatcher(saveDirectory, "save_slot_*.json");
            _fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime;
            _fileWatcher.EnableRaisingEvents = true;

            // ファイル変更イベント
            _fileWatcher.Changed += OnSaveFileChanged;
            _fileWatcher.Created += OnSaveFileChanged;
            _fileWatcher.Deleted += OnSaveFileChanged;

            Debug.Log("セーブファイル監視を開始しました。");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"ファイル監視の設定に失敗しました: {ex.Message}");
        }
    }

    /// <summary>
    /// セーブファイル変更時のコールバック
    /// </summary>
    private void OnSaveFileChanged(object sender, FileSystemEventArgs e)
    {
        // UIの更新はメインスレッドで実行する必要があるため、フラグを立てる
        _shouldUpdateUI = true;
    }

    private bool _shouldUpdateUI = false;

    public void MoveUpSlot()
    {
        _activeSlotIndex.Value = (_activeSlotIndex.Value - 1 + SaveConstants.MAX_SAVE_SLOTS) % SaveConstants.MAX_SAVE_SLOTS;
    }

    public void MoveDownSlot()
    {
        _activeSlotIndex.Value = (_activeSlotIndex.Value + 1) % SaveConstants.MAX_SAVE_SLOTS;
    }

    /// <summary>
    /// セーブデータUIのメニューのタイトル更新
    /// </summary>
    public void UpdateSaveTitleList()
    {
        var newList = new List<SaveList>();

        for (int i = 0; i < SaveConstants.MAX_SAVE_SLOTS; ++i)
        {
            SaveData saveData = _saveMgr.GetSaveData(i);
            SaveList saveList = new SaveList();

            if (saveData != null)
            {
                Date savedDate = new Date(saveData.DateData.Month, saveData.DateData.Day);
                string dateString = Date.Format(savedDate);
                int diffDays = Date.DiffDate(savedDate, Date.FirstDate);
                string location = SceneLocationManager.Instance.GetLocationDisplayNameFromSceneName(saveData.SystemData.CurrentSceneName);

                saveList.Date = saveData.SystemData.SystemDate;
                saveList.Title = $"{dateString}({(Date.IsEarlier(savedDate, Date.FirstDate) || Date.IsSameDate(savedDate, Date.FirstDate) ? (diffDays + 1) : (-diffDays - 1))}日目) - {location}";
            }
            else
            {
                saveList.Date = "0000/00/00 00:00";
                saveList.Title = SaveConstants.EMPTY_SLOT_TEXT;
            }

            newList.Add(saveList);
        }

        _saveTitleList.Value = newList;
    }

    public void Save()
    {
        _saveMgr.SaveToFile(_activeSlotIndex.Value);
        // セーブ後に少し待ってからUI更新（ファイル書き込み完了を待つ）
        StartCoroutine(DelayedUpdateUI());
    }

    /// <summary>
    /// 遅延してUI更新
    /// </summary>
    private System.Collections.IEnumerator DelayedUpdateUI()
    {
        yield return new WaitForSeconds(0.1f);
        UpdateSaveTitleList();
    }
}
