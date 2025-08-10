using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

public class EventSaveData : ISaveData
{
    [JsonProperty("current_story_layer")]
    public int CurrentStoryLayer { set; get; } = 0;

    [JsonProperty("event_data")]
    public Dictionary<string, EventData> EventData { set; get; } = new Dictionary<string, EventData>();

    public void DecodeToSaveData(string json)
    {
        try
        {
            EventSaveData data = JsonConvert.DeserializeObject<EventSaveData>(json);
            CurrentStoryLayer = data?.CurrentStoryLayer ?? 0;
            EventData = data?.EventData ?? new Dictionary<string, EventData>();
        }
        catch (JsonException ex)
        {
            Debug.LogError($"Eventのセーブデータのロードに失敗しました。: {ex.Message}");
            CurrentStoryLayer = 0;
            EventData = new Dictionary<string, EventData>();
        }
    }

    public string EncodeToJson()
    {
        var setting = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            Formatting = Formatting.Indented
        };
        return JsonConvert.SerializeObject(this, setting);
    }

    public string SaveDataName()
    {
        return "event";
    }
}