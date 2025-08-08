public interface ISaveData
{
    /// <summary>
    /// セーブするデータの名前(JSONの最初になる)
    /// </summary>
    /// <returns> 名前 </returns>
    public string SaveDataName();

    /// <summary>
    /// JSONにエンコードする
    /// </summary>
    /// <returns> JSON </returns>
    public string EncodeToJson();
    /// <summary>
    /// JSONからISaveDataにデコードする
    /// </summary>
    /// <param name="json"> JSON </param>
    public void DecodeToSaveData(string json);
}