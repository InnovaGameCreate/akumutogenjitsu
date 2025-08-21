using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _messageText;

    [Header("値")]
    [SerializeField] private string _name;
    [SerializeField] private string _message;

    private string _currentMessage;
    private string _currentName;

    [SerializeField] private Image _characterImage;
    private Sprite _currentSprite;

    public string Message
    {
        get => _currentMessage;
        set
        {
            if (_currentMessage == value)
            {
                return;
            }
            _currentMessage = value;
            _message = value;
            UpdateMessage();
        }
    }

    public string Name
    {
        get => _currentName;
        set
        {
            if (_currentName == value)
            {
                return;
            }
            _currentName = value;
            _name = value;
            UpdateName();
        }
    }

    public Sprite CharacterSprite
    {
        get => _currentSprite;
        set
        {
            if (_currentSprite == value) return;
            _currentSprite = value;
            UpdateSprite();
        }
    }

    private void UpdateSprite()
    {
        if (_characterImage == null) return;
        _characterImage.sprite = _currentSprite;
        _characterImage.gameObject.SetActive(_currentSprite != null);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_messageText == null)
        {
            Debug.LogError("_textTextがアサインされていません。");
            return;
        }
        if (_nameText == null)
        {
            Debug.LogError("_nameTextはアサインされていません。");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Message = _message;
        Name = _name;
    }

    private void UpdateMessage()
    {
        if (_messageText == null) return;
        _messageText.text = _currentMessage;
    }

    private void UpdateName()
    {
        if (_nameText == null) return;
        _nameText.text = _currentName;
    }
}
