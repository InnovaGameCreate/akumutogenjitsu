using UnityEngine;
using UnityEngine.UI;
using R3;

public class StartButtonView : MonoBehaviour
{
    [Header("UI設定")]
    [SerializeField] private Image _startButtonImage;
    [SerializeField] private Image _quitButtonImage;

    [Header("スプライト設定")]
    [SerializeField] private Sprite _selectedStartSprite;
    [SerializeField] private Sprite _notSelectedStartSprite;
    [SerializeField] private Sprite _selectedQuitSprite;
    [SerializeField] private Sprite _notSelectedQuitSprite;

    private readonly Subject<Unit> _moveLeft = new();
    public Observable<Unit> MoveLeft => _moveLeft;

    private readonly Subject<Unit> _moveRight = new();
    public Observable<Unit> MoveRight => _moveRight;

    private readonly Subject<int> _selectItem = new();
    public Observable<int> SelectItem => _selectItem;

    void Start()
    {
        SetupButtons();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) _moveLeft.OnNext(Unit.Default);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) _moveRight.OnNext(Unit.Default);
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            _selectItem.OnNext(-1); // 現在選択中のアイテム
    }

    private void SetupButtons()
    {
        if (_startButtonImage != null) SetupButton(_startButtonImage, () => _selectItem.OnNext(0));
        if (_quitButtonImage != null) SetupButton(_quitButtonImage, () => _selectItem.OnNext(1));
    }

    private void SetupButton(Image image, System.Action action)
    {
        var trigger = image.gameObject.GetComponent<UnityEngine.EventSystems.EventTrigger>();
        if (trigger == null)
        {
            trigger = image.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();
        }

        var entry = new UnityEngine.EventSystems.EventTrigger.Entry
        {
            eventID = UnityEngine.EventSystems.EventTriggerType.PointerClick
        };
        entry.callback.AddListener(_ => action());
        trigger.triggers.Add(entry);
        image.raycastTarget = true;
    }

    public void UpdateSelection(int index)
    {
        if (_startButtonImage != null)
            _startButtonImage.sprite = index == 0 ? _selectedStartSprite : _notSelectedStartSprite;

        if (_quitButtonImage != null)
            _quitButtonImage.sprite = index == 1 ? _selectedQuitSprite : _notSelectedQuitSprite;
    }

    void OnDestroy()
    {
        _moveLeft?.Dispose();
        _moveRight?.Dispose();
        _selectItem?.Dispose();
    }
}
