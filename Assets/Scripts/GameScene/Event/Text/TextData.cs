using UnityEngine;

[System.Serializable]
public class TextLine
{
    [Header("話者")]
    public string SpeakerName;
    public Sprite CharacterSprite;

    [Header("メッセージ")]
    [TextArea(3, 5)]
    public string Message;
}

[CreateAssetMenu(fileName = "TextData", menuName = "Game/Text")]
public class TextData : ScriptableObject
{
    [Header("会話データ")]
    public TextLine[] TextLines;

    [Header("1ページで表示する行数")]
    public int LinePerPage = 1;
}
