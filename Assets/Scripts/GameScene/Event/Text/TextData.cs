using UnityEngine;
[CreateAssetMenu(fileName = "TextData", menuName = "Game/Text")]
public class TextData : ScriptableObject
{
    [Header("会話データ")]
    public TextLine[] TextLines;

    [Header("1ページで表示する行数")]
    public int LinePerPage = 1;
}