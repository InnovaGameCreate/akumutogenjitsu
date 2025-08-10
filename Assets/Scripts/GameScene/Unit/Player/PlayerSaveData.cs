using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class PlayerSaveData : ISaveData
{
    [JsonProperty("position")]
    public Vector3 Position { get; set; }

    public void DecodeToSaveData(string json)
    {
        try
        {
            var data = JsonConvert.DeserializeObject<PlayerSaveData>(json);
            this.Position = data?.Position ?? Vector3.zero;
        }
        catch (JsonException ex)
        {
            Debug.LogError($"Failed to deserialize PlayerSaveData: {ex.Message}");
            this.Position = Vector3.zero;
        }
    }

    public string EncodeToJson()
    {
        // TODO: Formatting.Noneに変更する
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}