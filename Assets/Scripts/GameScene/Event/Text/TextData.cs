using UnityEngine;
[CreateAssetMenu(fileName = "TextData", menuName = "Game/Text")]
public class TextData : ScriptableObject
{
    [Header("��b�f�[�^")]
    public TextLine[] TextLines;

    [Header("1�y�[�W�ŕ\������s��")]
    public int LinePerPage = 1;
}