using System;
using Newtonsoft.Json;
using UnityEngine;

public class SaveData : ISaveDataBinary
{
    [JsonProperty("system")]
    public SystemSaveData SystemData { set; get; }

    [JsonProperty("date")]
    public DateSaveData DateData { set; get; }

    [JsonProperty("item")]
    public ItemSaveData ItemData { set; get; }

    [JsonProperty("event")]
    public EventSaveData EventData { set; get; }

    [JsonProperty("player")]
    public PlayerSaveData PlayerData { set; get; }

    public void DecodeToSaveData(string json)
    {
        try
        {
            var data = JsonConvert.DeserializeObject<SaveData>(json);
            SystemData = data?.SystemData ?? new SystemSaveData();
            DateData = data?.DateData ?? new DateSaveData();
            ItemData = data?.ItemData ?? new ItemSaveData();
            EventData = data?.EventData ?? new EventSaveData();
            PlayerData = data?.PlayerData ?? new PlayerSaveData();
        }
        catch (JsonException ex)
        {
            Debug.LogError($"セーブデータのデコードに失敗しました: {ex.Message}");
            SystemData = new SystemSaveData();
            DateData = new DateSaveData();
            ItemData = new ItemSaveData();
            EventData = new EventSaveData();
            PlayerData = new PlayerSaveData();
        }
    }

    public string EncodeToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    /// <summary>
    /// バイナリ（暗号化済み）にエンコード
    /// </summary>
    public byte[] EncodeToBinary()
    {
        string json = EncodeToJson();
        return SaveEncryption.EncryptJson(json);
    }

    /// <summary>
    /// バイナリ（暗号化済み）からデコード
    /// </summary>
    public void DecodeFromBinary(byte[] binaryData)
    {
        try
        {
            string json = SaveEncryption.DecryptToJson(binaryData);
            DecodeToSaveData(json);
        }
        catch (Exception ex)
        {
            Debug.LogError($"バイナリデータのデコードに失敗しました: {ex.Message}");
            // デフォルト値で初期化
            SystemData = new SystemSaveData();
            DateData = new DateSaveData();
            ItemData = new ItemSaveData();
            EventData = new EventSaveData();
            PlayerData = new PlayerSaveData();
        }
    }
}
