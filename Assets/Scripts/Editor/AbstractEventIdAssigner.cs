using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractEvent), true)]
public class AbstractEventIdAssigner : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var abstractEvent = (AbstractEvent)target;
        var so = new SerializedObject(abstractEvent);
        var idProp = so.FindProperty("_eventId");

        EditorGUILayout.Space(); // 少しスペースを空けて見やすくする

        // --- IDが未設定の場合 ---
        if (string.IsNullOrEmpty(idProp.stringValue))
        {
            if (GUILayout.Button("IDを自動生成"))
            {
                AssignNewId(idProp, so, abstractEvent);
            }
            EditorGUILayout.HelpBox("イベントIDが未設定です。自動生成してください。", MessageType.Warning);
        }
        // --- IDが設定済みの場合 ---
        else
        {
            // 既存のIDを表示
            EditorGUILayout.LabelField("イベントID", idProp.stringValue);

            // IDを強制的に再生成するボタンを追加
            if (GUILayout.Button("IDを再生成"))
            {
                // 誤操作防止の確認ダイアログ
                if (EditorUtility.DisplayDialog("IDの再生成の確認",
                    "本当にIDを再生成しますか？\nこのIDを参照している他の箇所がある場合、参照が壊れる可能性があります。", "はい、再生成します", "キャンセル"))
                {
                    AssignNewId(idProp, so, abstractEvent);
                }
            }
        }
    }

    /// <summary>
    /// 新しいGUIDを割り当てる共通処理
    /// </summary>
    private void AssignNewId(SerializedProperty idProp, SerializedObject so, Object targetObject)
    {
        idProp.stringValue = System.Guid.NewGuid().ToString();
        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(targetObject);
        Debug.Log($"新しいIDが {targetObject.name} に割り当てられました: {idProp.stringValue}");
    }
}