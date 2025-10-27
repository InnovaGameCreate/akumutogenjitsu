public interface ISaveData
{
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

public interface ISaveDataBinary : ISaveData
{
    byte[] EncodeToBinary();
    void DecodeFromBinary(byte[] binaryData);
}
