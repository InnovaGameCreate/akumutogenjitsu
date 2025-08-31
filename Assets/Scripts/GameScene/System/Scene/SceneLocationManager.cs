using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シーンと場所の名前を繋ぐ
/// </summary>
public class SceneLocationManager  : Singleton<SceneLocationManager>
{
    [SerializeField] private SceneList _sceneList;
    private Dictionary<string, eLocationType> _sceneToLocationTypes;
    private Dictionary<eLocationType, string> _locationTypeToDisplayNames;

    void Start()
    {
        InitializeDictionaries();
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
            _sceneToLocationTypes = new Dictionary<string, eLocationType>();
            _locationTypeToDisplayNames = new Dictionary<eLocationType, string>();

            // 重複を避けながら辞書を構築
            if (_sceneList.Scenes != null)
            {
                foreach (var scene in _sceneList.Scenes)
                {
                    if (!_sceneToLocationTypes.ContainsKey(scene.SceneName))
                    {
                        _sceneToLocationTypes[scene.SceneName] = scene.Type;
                    }
                    else
                    {
                        Debug.LogWarning($"重複するシーン名をスキップしました: {scene.SceneName}");
                    }
                }
            }

            if (_sceneList.Locations != null)
            {
                foreach (var location in _sceneList.Locations)
                {
                    _locationTypeToDisplayNames[location.Type] = location.DisplayName;
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"SceneListの初期化に失敗しました: {ex.Message}");
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
            return "■■■■■";
        }
    }
}