using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TextMeshProUGUI _dayText;
    [SerializeField] private Image _worldTypeIcon;
    [SerializeField] private TextMeshProUGUI _placeText;

    [Header("値")]
    [SerializeField] private int _month = 9;
    [SerializeField] private int _day = 10;
    [SerializeField] private eWorldType _worldType = eWorldType.Real;
    [SerializeField] private string _place;

    [Header("eWorldTypeのアイコン")]
    [SerializeField] private Sprite _realIcon;
    [SerializeField] private Sprite _dreamIcon;

    [Header("背景")]
    [SerializeField] private Sprite _realBackground;
    [SerializeField] private Sprite _dreamBackground;

    private int _currentMonth;
    private int _currentDay;
    private string _currentPlace;
    private eWorldType _currentWorldType;

    public int Month
    {
        get => _currentMonth;
        set
        {
            if (_currentMonth != value)
            {
                _currentMonth = value;
                UpdateDateText();
            }
        }
    }
    public int Day
    {
        get => _currentDay;
        set
        {
            if (_currentDay != value)
            {
                _currentDay = value;
                UpdateDateText();
            }
        }
    }
    public string Place
    {
        get => _currentPlace;
        set
        {
            if (_currentPlace != value)
            {
                _currentPlace = value;
                UpdatePlaceText();
            }
        }
    }
    public eWorldType WorldType
    {
        get => _currentWorldType;
        set
        {
            if (_currentWorldType != value)
            {
                _currentWorldType = value;
                UpdateWorldTypeImage();
            }
        }
    }

    void Start()
    {
        if (_dayText == null)
        {
            Debug.LogError("_dayTextObjがアサインされていません。");
            return;
        }
        if (_worldTypeIcon == null)
        {
            Debug.LogError("_worldTypeObjがアサインされていません。");
            return;
        }
        if (_placeText == null)
        {
            Debug.LogError("_placeTextObjがアサインされていません。");
            return;
        }
        UpdateDateText();
        UpdatePlaceText();
        UpdateWorldTypeImage();
    }

    void Update()
    {
        Month = _month;
        Day = _day;
        Place = _place;
        WorldType = _worldType;
    }

    private void UpdateDateText()
    {
        if (_dayText != null)
            _dayText.text = $"{_currentMonth}/{_currentDay}";
    }
    private void UpdatePlaceText()
    {
        if (_placeText != null)
            _placeText.text = _currentPlace;
    }
    private void UpdateWorldTypeImage()
    {
        if (_worldTypeIcon == null) return;
        switch (_currentWorldType)
        {
            case eWorldType.Real:
                _worldTypeIcon.sprite = _realIcon;
                _backgroundImage.sprite = _realBackground;
                break;
            case eWorldType.Dream:
                _worldTypeIcon.sprite = _dreamIcon;
                _backgroundImage.sprite = _dreamBackground;
                break;
            default:
                Debug.LogError($"_worldTypeが不正な値になりました。({_currentWorldType})");
                break;
        }
    }
}
