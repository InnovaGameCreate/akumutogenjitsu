public interface ISaveableManager<T> where T : ISaveData
{
    /// <summary>
    /// ISaveDataにエンコードする
    /// </summary>
    /// <returns> セーブデータ </returns>
    public T EncodeToSaveData();
    /// <summary>
    /// セーブデータをゲームに反映させる
    /// </summary>
    /// <param name="saveData"> セーブデータ </param>
    public void LoadFromSaveData(T saveData);
}