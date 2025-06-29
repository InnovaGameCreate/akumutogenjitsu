using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [Header("ボタン")]
    [SerializeField] private Image _saveButton;
    [SerializeField] private Image _titleButton;

    [Header("スプライト")]
    [SerializeField] private Sprite _selectedSaveSprite;
    [SerializeField] private Sprite _notSelectedSaveSprite;
    [SerializeField] private Sprite _selectedTitleSprite;
    [SerializeField] private Sprite _notSelectedTitleSprite;

    private int _selectedIndex = -1;

    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            if (value == _selectedIndex) return;

            _selectedIndex = value;
            UpdateButton();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void UpdateButton()
    {
        switch (_selectedIndex)
        {
            case 0:
                _saveButton.sprite = _selectedSaveSprite;
                _titleButton.sprite = _notSelectedTitleSprite;
                break;
            case 1:
                _saveButton.sprite = _notSelectedSaveSprite;
                _titleButton.sprite = _selectedTitleSprite;
                break;
            default:
                Debug.LogError($"不正な値が設定されています。({_selectedIndex})"); break;
        }
    }
}
