using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemManager : MonoBehaviour, ISaveableManager<SystemSaveData>
{
    [SerializeField] private GameObject _playerPrefab;
    void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.Log("Playerが存在しないので生成しました。");
            Instantiate(_playerPrefab);
        }
    }

    // MARK: セーブ機能

    public SystemSaveData EncodeToSaveData()
    {
        SystemSaveData saveData = new SystemSaveData();
        saveData.CurrentSceneName = SceneManager.GetActiveScene().name;
        saveData.SystemDate = DateTime.Now.ToString("yyyy/M/dd H:mm");
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
