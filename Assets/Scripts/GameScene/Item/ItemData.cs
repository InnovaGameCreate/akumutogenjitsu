using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    [Header("アイテムの基本情報")]
    [SerializeField] private eItem _itemType;
    [SerializeField] private string _name;
    [TextArea(3, 10), SerializeField]
    private string _description;
    [SerializeField] private Sprite _icon;

    public eItem ItemType => _itemType;
    public string Name => _name;
    public string Description => _description;
    public Sprite Icon => _icon;
}
