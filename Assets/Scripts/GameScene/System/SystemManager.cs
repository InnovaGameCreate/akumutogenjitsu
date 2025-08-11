using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemManager : MonoBehaviour, ISaveableManager<SystemSaveData>
{
    // MARK: セーブ機能

    public SystemSaveData EncodeToSaveData()
    {
        SystemSaveData saveData = new SystemSaveData();
        saveData.CurrentSceneName = SceneManager.GetActiveScene().name;
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
