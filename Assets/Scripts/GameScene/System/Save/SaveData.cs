using Newtonsoft.Json;
using UnityEngine;

public class SaveData : ISaveData
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
}