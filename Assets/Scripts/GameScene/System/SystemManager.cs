using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemManager : MonoBehaviour, ISaveableManager<SystemSaveData>
{
    // MARK: セーブ機能

    public SystemSaveData EncodeToSaveData()
    {
        SystemSaveData saveData = new SystemSaveData();
        saveData.CurrentSceneName = SceneManager.GetActiveScene().name;
        string year = DateTime.Now.Year.ToString();
        string month = DateTime.Now.Month.ToString();
        string day = DateTime.Now.Day < 10 ? $"0{DateTime.Now.Day}" : DateTime.Now.Day.ToString();
        string hour = DateTime.Now.Hour.ToString();
        string minute = DateTime.Now.Minute < 10 ? $"0{DateTime.Now.Minute}" : DateTime.Now.Minute.ToString();
        saveData.SystemDate = $"{year}/{month}/{day} {hour}:{minute}";
        return saveData;
    }

    public void LoadFromSaveData(SystemSaveData saveData)
    {
        if (!string.IsNullOrEmpty(saveData?.CurrentSceneName))
        {
            SceneManager.LoadScene(saveData.CurrentSceneName);
        }
        else
        {
            Debug.LogError("セーブデータからシーンをロードできませんでした。");
        }
    }
}
