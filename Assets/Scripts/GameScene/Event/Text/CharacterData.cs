using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Game/Character")]

public class CharacterData : ScriptableObject

{

    [SerializeField] private eCharacter _character;

    [SerializeField] private string _characterName;

    [SerializeField] private Sprite _characterSprite;

    [SerializeField] private Color _textColor = Color.white;

    public eCharacter Character => _character;

    public string CharacterName => _characterName;

    public Sprite CharacterSprite => _characterSprite;

    public Color TextColor => _textColor;

}