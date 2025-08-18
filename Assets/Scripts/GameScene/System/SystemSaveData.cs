using System;
using Newtonsoft.Json;
using UnityEngine;

public class SystemSaveData : ISaveData
{
    [JsonProperty("current_scene")]
    public string CurrentSceneName { get; set; } = "";

    [JsonProperty("system_date")]
    public string SystemDate { set; get; } = "";

    public void DecodeToSaveData(string json)
    {
        try
        {
            SystemSaveData data = JsonConvert.DeserializeObject<SystemSaveData>(json);
            this.CurrentSceneName = data?.CurrentSceneName ?? "";
            this.SystemDate = data?.SystemDate;
        }
        catch (JsonException ex)
        {
            Debug.LogError($"Systemのセーブデータの生成に失敗しました。 {ex.Message}");
            CurrentSceneName = "";
            SystemDate = "0000/00/00 00:00";
        }
    }

    public string EncodeToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}