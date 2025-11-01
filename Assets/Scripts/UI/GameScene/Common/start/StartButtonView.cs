using UnityEngine;
using UnityEngine.UI;
using R3;

public class StartButtonView : MonoBehaviour
{
    [Header("UI設定")]
    [SerializeField] private Image _startButtonImage;
    [SerializeField] private Image _loadButtonImage;
    [SerializeField] private Image _quitButtonImage;

    [Header("スプライト設定")]
    [SerializeField] private Sprite _selectedStartSprite;
    [SerializeField] private Sprite _notSelectedStartSprite;
    [SerializeField] private Sprite _selectedLoadSprite;
    [SerializeField] private Sprite _notSelectedLoadSprite;
    [SerializeField] private Sprite _selectedQuitSprite;
    [SerializeField] private Sprite _notSelectedQuitSprite;

    private readonly Subject<Unit> _moveUp = new();
    public Observable<Unit> MoveUp => _moveUp;

    private readonly Subject<Unit> _moveDown = new();
    public Observable<Unit> MoveDown => _moveDown;

    private readonly Subject<int> _selectItem = new();
    public Observable<int> SelectItem => _selectItem;

    void Start()
    {
        SetupButtons();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) _moveUp.OnNext(Unit.Default);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) _moveDown.OnNext(Unit.Default);
        else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
            _selectItem.OnNext(-1); // 現在選択中のアイテム
    }

    private void SetupButtons()
    {
        if (_startButtonImage != null) SetupButton(_startButtonImage, () => _selectItem.OnNext(0));
        if (_loadButtonImage != null) SetupButton(_loadButtonImage, () => _selectItem.OnNext(1));
        if (_quitButtonImage != null) SetupButton(_quitButtonImage, () => _selectItem.OnNext(2));
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

        if (_loadButtonImage != null)
            _loadButtonImage.sprite = index == 1 ? _selectedLoadSprite : _notSelectedLoadSprite;

        if (_quitButtonImage != null)
            _quitButtonImage.sprite = index == 2 ? _selectedQuitSprite : _notSelectedQuitSprite;
    }

    void OnDestroy()
    {
        _moveUp?.Dispose();
        _moveDown?.Dispose();
        _selectItem?.Dispose();
    }
}
