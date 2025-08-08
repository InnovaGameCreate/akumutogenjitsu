using UnityEngine;
using UnityEngine.UIElements;
using Newtonsoft.Json;

[System.Serializable]
class PlayerSaveData : ISaveData
{
    [JsonProperty("position")]
    public Vector2 Position { get; set; }

    public void DecodeToSaveData(string json)
    {
        var data = JsonConvert.DeserializeObject<PlayerSaveData>(json);
        this.Position = data.Position;
    }

    public string EncodeToJson()
    {
        // TODO: Formatting.Noneに変更する
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    public string SaveDataName()
    {
        return "Player";
    }
}