using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordEventSlotView : MonoBehaviour
{
    [Header("矢印")]
    [SerializeField] private Image _arrowPlusImage;
    [SerializeField] private Image _arrowMinusImage;

    [Header("数字")]
    [SerializeField] private TextMeshProUGUI _number;

    [Header("素材")]
    [SerializeField] private Sprite _selectedPlusArrowSprite;
    [SerializeField] private Sprite _notSelectedPlusArrowSprite;
    [SerializeField] private Sprite _selectedMinusArrowSprite;
    [SerializeField] private Sprite _notSelectedMinusArrowSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_number == null)
        {
            Debug.LogError("_numberがnullです。");
            return;
        }
        _number.text = "0";
    }

    /// <summary>
    /// 数字を設定する
    /// </summary>
    /// <param name="num"> 接待したい数字 </param>
    public void SetNumber(int num)
    {
        if (num < 0 || num > 9)
        {
            Debug.LogError("不正な値が渡されました。");
            return;
        }
        _number.text = num.ToString();
    }

    /// <summary>
    /// アクティブにするか
    /// </summary>
    public void SetActiveSlot(bool isActive)
    {
        if (isActive)
        {
            _arrowPlusImage.sprite = _selectedPlusArrowSprite;
            _arrowMinusImage.sprite = _selectedMinusArrowSprite;
        }
        else
        {
            _arrowPlusImage.sprite = _notSelectedPlusArrowSprite;
            _arrowMinusImage.sprite = _notSelectedMinusArrowSprite;
        }
    }
}
