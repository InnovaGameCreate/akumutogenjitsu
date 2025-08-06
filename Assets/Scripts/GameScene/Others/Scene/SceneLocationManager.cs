using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// シーンと場所の名前を繋ぐ
/// </summary>
public class SceneLocationManager  : Singleton<SceneLocationManager >
{
    [SerializeField] private SceneList _sceneList;
    private Dictionary<string, eLocationType> _sceneToLocationTypes;
    private Dictionary<eLocationType, string> _locationTypeToDisplayNames;

    void Awake()
    {
        if (CheckInstance())
        {
            InitializeDictionaries();
        }
    }

    private void InitializeDictionaries()
    {
        if (_sceneList == null)
        {
            Debug.LogError("SceneListがアサインされていません。");
            return;
        }

        try
        {
            _sceneToLocationTypes = _sceneList.Scenes?
                .ToDictionary(scene => scene.SceneName, scene => scene.Type) ?? new Dictionary<string, eLocationType>();
            _locationTypeToDisplayNames = _sceneList.Locations?
                .ToDictionary(scene => scene.Type, scene => scene.DisplayName) ?? new Dictionary<eLocationType, string>();
        }
        catch (System.ArgumentException ex)
        {
            Debug.LogError($"SceneListに重複するキーが存在します: {ex.Message}");
            _sceneToLocationTypes = new Dictionary<string, eLocationType>();
            _locationTypeToDisplayNames = new Dictionary<eLocationType, string>();
        }
    }

    public string GetLocationDisplayNameFromSceneName(string sceneName)
    {
        if (_sceneToLocationTypes != null && _locationTypeToDisplayNames != null &&
            _sceneToLocationTypes.TryGetValue(sceneName, out var type) && 
            _locationTypeToDisplayNames.TryGetValue(type, out var name))
        {
            return name;
        }
        else
        {
            Debug.LogError("不正なシーン名が指定されました。");
            return "不明なシーン";
        }
    }
}