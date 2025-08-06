using System;
using System.Collections.Generic;
using UnityEngine;

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
    private void OnValidate()
    {
        if (_sceneAsset != null)
        {
            _sceneName = _sceneAsset.name;
        }
    }

    [ContextMenu("シーン名を更新")]
    public void UpdateSceneName()
    {
        OnValidate();
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
        foreach (var scene in Scenes)
        {
            scene.UpdateSceneName();
        }
        
        // 変更をUnityに通知
        UnityEditor.EditorUtility.SetDirty(this);
        Debug.Log("全シーン名を更新しました");
    }
    
    private void OnValidate()
    {
        UpdateAllSceneNames();
    }
#endif
}