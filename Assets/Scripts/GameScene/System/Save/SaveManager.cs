using System.IO;
using UnityEngine;
using VContainer;

public class SaveManager : ISaveableManager<SaveData>
{
    /// <summary>
    /// 最大セーブスロット数
    /// </summary>
    public const int MAX_SAVE_SLOTS = 3;
    
    private readonly SystemManager _systemMgr;
    private readonly EventManager _eventMgr;
    private readonly DateManager _dateMgr;
    private readonly ItemManager _itemMgr;
    private readonly PlayerManager _playerMgr;

    [Inject]
    public SaveManager(
        SystemManager systemMgr, 
        EventManager eventMgr, 
        DateManager dateMgr, 
        ItemManager itemMgr, 
        PlayerManager playerMgr)
    {
        _systemMgr = systemMgr;
        _eventMgr = eventMgr;
        _dateMgr = dateMgr;
        _itemMgr = itemMgr;
        _playerMgr = playerMgr;
    }

    public SaveData EncodeToSaveData()
    {
        SaveData saveData = new SaveData();
        saveData.SystemData = _systemMgr.EncodeToSaveData();
        saveData.DateData = _dateMgr.EncodeToSaveData();
        saveData.EventData = _eventMgr.EncodeToSaveData();
        saveData.ItemData = _itemMgr.EncodeToSaveData();
        saveData.PlayerData = _playerMgr.EncodeToSaveData();
        return saveData;
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        _systemMgr.LoadFromSaveData(saveData.SystemData);
        _eventMgr.LoadFromSaveData(saveData.EventData);
        _dateMgr.LoadFromSaveData(saveData.DateData);
        _itemMgr.LoadFromSaveData(saveData.ItemData);
        _playerMgr.LoadFromSaveData(saveData.PlayerData);
    }

    /// <summary>
    /// JSON形式のセーブデータの生成
    /// </summary>
    /// <returns> JSON </returns>
    public string CreateSaveDataJson()
    {
        return EncodeToSaveData().EncodeToJson();
    }

    /// <summary>
    /// 指定されたスロットにセーブデータを保存する
    /// </summary>
    /// <param name="slotNumber">セーブスロット番号（1-3）</param>
    public void SaveToFile(int slotNumber)
    {
        // スロット番号の検証
        if (slotNumber < 1 || slotNumber > MAX_SAVE_SLOTS)
        {
            Debug.LogError($"無効なスロット番号です。1から{MAX_SAVE_SLOTS}の間で指定してください。(入力値: {slotNumber})");
            return;
        }

        try
        {
            string filePath = GetSaveFilePath(slotNumber);
            
            // セーブデータのJSON文字列を取得
            string saveDataJson = CreateSaveDataJson();
            
            // ファイルに書き込み
            File.WriteAllText(filePath, saveDataJson);
            
            Debug.Log($"スロット{slotNumber}にセーブデータを保存しました: {filePath}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"スロット{slotNumber}へのセーブデータ保存に失敗しました: {ex.Message}");
        }
    }

    /// <summary>
    /// 指定されたスロットからセーブデータを読み込む
    /// </summary>
    /// <param name="slotNumber">ロードするスロット番号（1-3）</param>
    /// <returns>読み込みが成功したかどうか</returns>
    public bool LoadFromFile(int slotNumber)
    {
        // スロット番号の検証
        if (slotNumber < 1 || slotNumber > MAX_SAVE_SLOTS)
        {
            Debug.LogError($"無効なスロット番号です。1から{MAX_SAVE_SLOTS}の間で指定してください。(入力値: {slotNumber})");
            return false;
        }

        try
        {
            string filePath = GetSaveFilePath(slotNumber);
            
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"スロット{slotNumber}にセーブデータが存在しません: {filePath}");
                return false;
            }

            string saveDataJson = File.ReadAllText(filePath);
            SaveData saveData = new SaveData();
            saveData.DecodeToSaveData(saveDataJson);
            
            LoadFromSaveData(saveData);
            
            Debug.Log($"スロット{slotNumber}からセーブデータを読み込みました: {filePath}");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"スロット{slotNumber}からのセーブデータ読み込みに失敗しました: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 指定されたスロットにセーブデータが存在するかチェック
    /// </summary>
    /// <param name="slotNumber">チェックするスロット番号（1-3）</param>
    /// <returns>セーブデータが存在するかどうか</returns>
    public bool HasSaveData(int slotNumber)
    {
        if (slotNumber < 1 || slotNumber > MAX_SAVE_SLOTS)
        {
            return false;
        }

        string filePath = GetSaveFilePath(slotNumber);
        return File.Exists(filePath);
    }

    /// <summary>
    /// 指定されたスロットのセーブデータを削除
    /// </summary>
    /// <param name="slotNumber">削除するスロット番号（1-3）</param>
    /// <returns>削除が成功したかどうか</returns>
    public bool DeleteSaveData(int slotNumber)
    {
        if (slotNumber < 1 || slotNumber > MAX_SAVE_SLOTS)
        {
            Debug.LogError($"無効なスロット番号です。1から{MAX_SAVE_SLOTS}の間で指定してください。(入力値: {slotNumber})");
            return false;
        }

        try
        {
            string filePath = GetSaveFilePath(slotNumber);
            
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"スロット{slotNumber}にセーブデータが存在しません: {filePath}");
                return false;
            }

            File.Delete(filePath);
            Debug.Log($"スロット{slotNumber}のセーブデータを削除しました: {filePath}");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"スロット{slotNumber}のセーブデータ削除に失敗しました: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 指定されたスロット番号に対応するファイルパスを取得
    /// </summary>
    /// <param name="slotNumber">スロット番号（1-3）</param>
    /// <returns>ファイルパス</returns>
    private string GetSaveFilePath(int slotNumber)
    {
        string saveDirectory = Path.Combine(Application.persistentDataPath, "Saves");
        
        // ディレクトリが存在しない場合は作成
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
        
        return Path.Combine(saveDirectory, $"save_slot_{slotNumber}.json");
    }
}