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

    [SerializeField] SceneAsset _sceneAsset;
    public string SceneName => _sceneAsset?.name;
}

[CreateAssetMenu(fileName = "SceneList", menuName = "Game/SceneList")]
public class SceneList : ScriptableObject
{
    [Header("場所の定義")]
    public List<LocationDefine> Locations;

    [Header("シーンの定義")]
    public List<SceneDefine> Scenes;
}