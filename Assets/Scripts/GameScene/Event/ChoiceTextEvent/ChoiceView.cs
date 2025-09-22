using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TextMeshProUGUI _text;

    [Header("Sprite")]
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private Sprite _notSelectedSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_backgroundImage == null)
        {
            Debug.LogError("BackgroundImageがアタッチされていません。");
            return;
        }
        if (_text == null)
        {
            Debug.LogError("Textがアタッチされていません。");
            return;
        }
    }

    public void SetChoice(Choice choice)
    {
        _text.text = choice.ChoiceText;
    }

    public void SetIsSelected(bool selected)
    {
        _backgroundImage.sprite = selected ? _selectedSprite : _notSelectedSprite;
    }
}
