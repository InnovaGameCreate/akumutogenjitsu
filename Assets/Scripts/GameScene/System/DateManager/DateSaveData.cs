using Newtonsoft.Json;
using UnityEngine;

public class DateSaveData : ISaveData
{
    [JsonProperty("month")]
    public int Month { set; get; } = 9;
    [JsonProperty("day")]
    public int Day { set; get; } = 6;

    public void DecodeToSaveData(string json)
    {
        try
        {
            DateSaveData data = JsonConvert.DeserializeObject<DateSaveData>(json);
            this.Month = data?.Month ?? 9;
            this.Day = data?.Day ?? 6;
        }
        catch (JsonException ex)
        {
            Debug.LogError($"Dateのセーブデータの生成に失敗しました。 {ex.Message}");
            this.Month = 9;
            this.Day = 6;
        }
    }

    public string EncodeToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    public string SaveDataName()
    {
        return "date";
    }
}