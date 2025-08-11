using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

[System.Serializable]
public class PlayerSaveData : ISaveData
{
    [JsonProperty("position")]
    public SerializableVector3 Position { get; set; }

    public void DecodeToSaveData(string json)
    {
        try
        {
            var data = JsonConvert.DeserializeObject<PlayerSaveData>(json);
            this.Position = data?.Position ?? new SerializableVector3(Vector3.zero);
        }
        catch (JsonException ex)
        {
            Debug.LogError($"PlayerSaveDataのデシリアライズに失敗しました: {ex.Message}");
            this.Position = new SerializableVector3(Vector3.zero);
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
        // TODO: Formatting.Noneに変更する
        return JsonConvert.SerializeObject(this, setting);
    }
}
