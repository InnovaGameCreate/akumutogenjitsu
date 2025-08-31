using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;


#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 場所の種類
/// </summary>
public enum eLocationType
{
    None,
    House,
    Street,

    // 学校
    School_1_Entrance,
    School_1_Class,
    School_1_Science,
    School_1_TeacherRoom,
    School_2_Class,
    School_2_Hallway,
}

/// <summary>
/// 場所の名前
/// </summary>
[Serializable]
public class LocationDefine
{
    public eLocationType Type;
    public string DisplayName;
}

[Serializable]
public class SceneDefine
{
    public eLocationType Type;

#if UNITY_EDITOR
    [SerializeField] private SceneAsset _sceneAsset;
#endif

    [SerializeField] private string _sceneName;
    public string SceneName => _sceneName;

#if UNITY_EDITOR
    public void UpdateSceneName()
    {
        if (_sceneAsset != null)
        {
            _sceneName = _sceneAsset.name;
        }
    }
#endif
}

[CreateAssetMenu(fileName = "SceneList", menuName = "Game/SceneList")]
public class SceneList : ScriptableObject
{
    public List<LocationDefine> Locations = new List<LocationDefine>();
    public List<SceneDefine> Scenes = new List<SceneDefine>();

#if UNITY_EDITOR
    [ContextMenu("全シーン名を更新")]
    private void UpdateAllSceneNames()
    {
        bool hasChanged = false;
        
        foreach (var scene in Scenes)
        {
            string oldName = scene.SceneName;
            scene.UpdateSceneName();
            
            if (oldName != scene.SceneName)
            {
                hasChanged = true;
            }
        }
        
        if (hasChanged)
        {
            EditorUtility.SetDirty(this);
            Debug.Log("シーン名を更新しました");
        }
    }

    private void OnValidate()
    {
        if (Scenes == null) Scenes = new List<SceneDefine>();
        if (Locations == null) Locations = new List<LocationDefine>();

        UpdateAllSceneNames();
    }
#endif
}