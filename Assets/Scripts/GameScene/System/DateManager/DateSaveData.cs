using Newtonsoft.Json;
using UnityEngine;

public class DateSaveData : ISaveData
{
    [JsonProperty("month")]
    public int Month { set; get; } = Date.FirstDate.Month;
    [JsonProperty("day")]
    public int Day { set; get; } = Date.FirstDate.Day;

    public void DecodeToSaveData(string json)
    {
        try
        {
            DateSaveData data = JsonConvert.DeserializeObject<DateSaveData>(json);
            this.Month = data?.Month ?? Date.FirstDate.Month;
            this.Day = data?.Day ?? Date.FirstDate.Day;
        }
        catch (JsonException ex)
        {
            Debug.LogError($"Dateのセーブデータの生成に失敗しました。 {ex.Message}");
            this.Month = Date.FirstDate.Month;
            this.Day = Date.FirstDate.Day;
        }
    }

    public string EncodeToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}