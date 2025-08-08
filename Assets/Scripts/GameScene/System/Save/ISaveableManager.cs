public interface ISaveableManager
{
    /// <summary>
    /// ISaveDataにエンコードする
    /// </summary>
    /// <returns> セーブデータ </returns>
    ISaveData EncodeToSaveData();
    /// <summary>
    /// セーブデータをゲームに反映させる
    /// </summary>
    /// <param name="saveData"> セーブデータ </param>
    void LoadFromSaveData(ISaveData saveData);
}