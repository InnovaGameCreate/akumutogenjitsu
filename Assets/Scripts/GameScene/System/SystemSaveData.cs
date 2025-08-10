using Newtonsoft.Json;
using UnityEngine;

public class SystemSaveData : ISaveData
{
    [JsonProperty("current_scene")]
    public string CurrentSceneName { get; set; } = "";

    public void DecodeToSaveData(string json)
    {
        try
        {
            SystemSaveData data = JsonConvert.DeserializeObject<SystemSaveData>(json);
            this.CurrentSceneName = data?.CurrentSceneName ?? "";
        }
        catch (JsonException ex)
        {
            Debug.LogError($"Systemのセーブデータの生成に失敗しました。 {ex.Message}");
            CurrentSceneName = "";
        }
    }

    public string EncodeToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}