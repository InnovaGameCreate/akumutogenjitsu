using System;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour, ISaveableManager<SaveData>
{
    /// <summary>
    /// 最大セーブスロット数
    /// </summary>
    public const int MAX_SAVE_SLOTS = 3;

    /// <summary>
    /// 暗号化を使用するかどうか（Inspector で設定可能）
    /// </summary>
    [SerializeField] private bool useEncryption = true;

    private SystemManager _systemMgr;
    private EventManager _eventMgr;
    private DateManager _dateMgr;
    private PlayerManager _playerMgr;
    private ItemManager _itemMgr;

    void Start()
    {
        InitializeManagers();
    }

    private void InitializeManagers()
    {
        GameObject systemMgrObj = GameObject.FindWithTag("SystemMgr");
        GameObject eventMgrObj = GameObject.FindWithTag("EventMgr");
        GameObject dateMgrObj = GameObject.FindWithTag("DateMgr");
        GameObject playerMgrObj = GameObject.FindWithTag("PlayerMgr");

        if (systemMgrObj == null)
        {
            Debug.LogError("SystemMgrが存在しません。");
            return;
        }
        if (eventMgrObj == null)
        {
            Debug.LogError("EventMgrが存在しません。");
            return;
        }
        if (dateMgrObj == null)
        {
            Debug.LogError("DateMgrが存在しません。");
            return;
        }
        if (playerMgrObj == null)
        {
            Debug.LogError("PlayerMgrが存在しません。");
            return;
        }

        _systemMgr = systemMgrObj.GetComponent<SystemManager>();
        _eventMgr = eventMgrObj.GetComponent<EventManager>();
        _dateMgr = dateMgrObj.GetComponent<DateManager>();
        _playerMgr = playerMgrObj.GetComponent<PlayerManager>();

        if (_systemMgr == null)
        {
            Debug.LogError("SystemManagerがコンポーネントされていません。");
        }
        if (_eventMgr == null)
        {
            Debug.LogError("EventManagerがコンポーネントされていません。");
        }
        if (_dateMgr == null)
        {
            Debug.LogError("DateManagerがコンポーネントされていません。");
        }
        if (_playerMgr == null)
        {
            Debug.LogError("PlayerManagerがコンポーネントされていません。");
        }
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
    /// 指定されたスロットからSaveData形式でセーブデータを取得する
    /// </summary>
    /// <param name="slotNumber">スロット番号（0-2）</param>
    /// <returns>SaveData形式のデータ。失敗時はnull</returns>
    public SaveData GetSaveData(int slotNumber)
    {
        // スロット番号の検証
        if (slotNumber < 0 || slotNumber >= MAX_SAVE_SLOTS)
        {
            Debug.LogError($"無効なスロット番号です。0から{MAX_SAVE_SLOTS - 1}の間で指定してください。(入力値: {slotNumber})");
            return null;
        }

        try
        {
            string filePath = GetSaveFilePath(slotNumber);

            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"スロット{slotNumber}にセーブデータが存在しません: {filePath}");
                return null;
            }

            SaveData saveData = new SaveData();

            if (useEncryption)
            {
                // 暗号化モード: バイナリとして読み込み
                byte[] encryptedData = File.ReadAllBytes(filePath);
                saveData.DecodeFromBinary(encryptedData);
            }
            else
            {
                // 非暗号化モード: JSONとして読み込み
                string saveDataJson = File.ReadAllText(filePath);
                saveData.DecodeToSaveData(saveDataJson);
            }

            return saveData;
        }
        catch (Exception ex)
        {
            Debug.LogError($"スロット{slotNumber}からのセーブデータ取得に失敗しました: {ex.Message}");
            return null;
        }
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
    /// <param name="slotNumber">セーブスロット番号（0-2）</param>
    public void SaveToFile(int slotNumber)
    {
        // スロット番号の検証
        if (slotNumber < 0 || slotNumber >= MAX_SAVE_SLOTS)
        {
            Debug.LogError($"無効なスロット番号です。0から{MAX_SAVE_SLOTS - 1}の間で指定してください。(入力値: {slotNumber})");
            return;
        }

        try
        {
            string filePath = GetSaveFilePath(slotNumber);
            SaveData saveData = EncodeToSaveData();

            if (useEncryption)
            {
                // 暗号化モード: バイナリとして保存
                byte[] encryptedData = saveData.EncodeToBinary();
                File.WriteAllBytes(filePath, encryptedData);
                Debug.Log($"スロット{slotNumber}に暗号化されたセーブデータを保存しました: {filePath}");
            }
            else
            {
                // 非暗号化モード: JSONとして保存
                string saveDataJson = saveData.EncodeToJson();
                File.WriteAllText(filePath, saveDataJson);
                Debug.Log($"スロット{slotNumber}にセーブデータを保存しました: {filePath}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"スロット{slotNumber}へのセーブデータ保存に失敗しました: {ex.Message}");
        }
    }

    /// <summary>
    /// 指定されたスロットからセーブデータを読み込む
    /// </summary>
    /// <param name="slotNumber">ロードするスロット番号（0-2）</param>
    /// <returns>読み込みが成功したかどうか</returns>
    public bool LoadFromFile(int slotNumber)
    {
        SaveData saveData = GetSaveData(slotNumber);

        if (saveData == null)
        {
            return false;
        }

        try
        {
            LoadFromSaveData(saveData);
            Debug.Log($"スロット{slotNumber}からセーブデータを読み込みました");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"スロット{slotNumber}のセーブデータ読み込み処理に失敗しました: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 指定されたスロットにセーブデータが存在するかチェック
    /// </summary>
    /// <param name="slotNumber">チェックするスロット番号（0-2）</param>
    /// <returns>セーブデータが存在するかどうか</returns>
    public bool HasSaveData(int slotNumber)
    {
        if (slotNumber < 0 || slotNumber >= MAX_SAVE_SLOTS)
        {
            return false;
        }

        string filePath = GetSaveFilePath(slotNumber);
        return File.Exists(filePath);
    }

    /// <summary>
    /// 指定されたスロットのセーブデータを削除
    /// </summary>
    /// <param name="slotNumber">削除するスロット番号（0-2）</param>
    /// <returns>削除が成功したかどうか</returns>
    public bool DeleteSaveData(int slotNumber)
    {
        if (slotNumber < 0 || slotNumber >= MAX_SAVE_SLOTS)
        {
            Debug.LogError($"無効なスロット番号です。0から{MAX_SAVE_SLOTS - 1}の間で指定してください。(入力値: {slotNumber})");
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
        catch (Exception ex)
        {
            Debug.LogError($"スロット{slotNumber}のセーブデータ削除に失敗しました: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 指定されたスロット番号に対応するファイルパスを取得
    /// </summary>
    /// <param name="slotNumber">スロット番号（0-2）</param>
    /// <returns>ファイルパス</returns>
    private string GetSaveFilePath(int slotNumber)
    {
        string saveDirectory = Path.Combine(Application.persistentDataPath, "Saves");

        // ディレクトリが存在しない場合は作成
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }

        // 暗号化モードでは拡張子を.datに変更
        string extension = useEncryption ? "dat" : "json";
        return Path.Combine(saveDirectory, $"save_slot_{slotNumber}.{extension}");
    }
}
