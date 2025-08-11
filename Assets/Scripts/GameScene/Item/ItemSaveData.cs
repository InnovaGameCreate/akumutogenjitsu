using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class ItemSaveData : ISaveData
{
    [JsonProperty("item")]
    public List<eItem> OwnedItems { get; set; } = new List<eItem>();

    public void DecodeToSaveData(string json)
    {
        try
        {
            ItemSaveData data = JsonConvert.DeserializeObject<ItemSaveData>(json);
            this.OwnedItems = data?.OwnedItems ?? new List<eItem>();
        }
        catch (JsonException ex)
        {
            Debug.LogError($"Itemのセーブデータの生成に失敗しました。 {ex.Message}");
            this.OwnedItems = new List<eItem>();
        }
    }

    public string EncodeToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}