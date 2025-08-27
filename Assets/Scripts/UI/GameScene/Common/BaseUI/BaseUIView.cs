using TMPro;
using UnityEngine;
using UnityEngine.UI;
using R3;

public class BaseUIView : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TextMeshProUGUI _dayText;
    [SerializeField] private Image _worldTypeIcon;
    [SerializeField] private TextMeshProUGUI _placeText;

    [Header("World Type Icons")]
    [SerializeField] private Sprite _realIcon;
    [SerializeField] private Sprite _dreamIcon;

    [Header("Backgrounds")]
    [SerializeField] private Sprite _realBackground;
    [SerializeField] private Sprite _dreamBackground;

    void Start()
    {
        ValidateComponents();
    }

    private void ValidateComponents()
    {
        if (_dayText == null)
        {
            Debug.LogError("_dayTextがアサインされていません。");
        }
        if (_worldTypeIcon == null)
        {
            Debug.LogError("_worldTypeIconがアサインされていません。");
        }
        if (_placeText == null)
        {
            Debug.LogError("_placeTextがアサインされていません。");
        }
        if (_backgroundImage == null)
        {
            Debug.LogError("_backgroundImageがアサインされていません。");
        }
    }

    public void UpdateDateText(int month, int day)
    {
        if (_dayText != null)
        {
            _dayText.text = $"{month}/{day}";
        }
    }

    public void UpdatePlaceText(string place)
    {
        if (_placeText != null)
        {
            _placeText.text = place;
        }
    }

    public void UpdateWorldTypeDisplay(eWorldType worldType)
    {
        UpdateWorldTypeIcon(worldType);
        UpdateBackground(worldType);
    }

    private void UpdateWorldTypeIcon(eWorldType worldType)
    {
        if (_worldTypeIcon == null) return;

        switch (worldType)
        {
            case eWorldType.Real:
                _worldTypeIcon.sprite = _realIcon;
                break;
            case eWorldType.Dream:
                _worldTypeIcon.sprite = _dreamIcon;
                break;
            default:
                Debug.LogError($"不正なWorldType値です: {worldType}");
                break;
        }
    }

    private void UpdateBackground(eWorldType worldType)
    {
        if (_backgroundImage == null) return;

        switch (worldType)
        {
            case eWorldType.Real:
                _backgroundImage.sprite = _realBackground;
                break;
            case eWorldType.Dream:
                _backgroundImage.sprite = _dreamBackground;
                break;
            default:
                Debug.LogError($"不正なWorldType値です: {worldType}");
                break;
        }
    }
}
