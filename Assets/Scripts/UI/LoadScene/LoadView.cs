using System.Collections.Generic;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct LoadViewOutput
{
    public Observable<Unit> moveUp;
    public Observable<Unit> moveDown;
    public Observable<Unit> select;
    public Observable<Unit> backTitle;
}

[System.Serializable]
class LoadSlotObj
{
    public Image backgroundImg;
    public TextMeshProUGUI date;
    public TextMeshProUGUI title;
}

public class LoadView : MonoBehaviour
{
    [SerializeField] private List<LoadSlotObj> _loadSlotObjs;

    [Header("素材")]
    [SerializeField] private Sprite _activeSlot;
    [SerializeField] private Sprite _notActiveSlot;

    private Subject<Unit> _moveUp = new();
    private Subject<Unit> _moveDown = new();
    private Subject<Unit> _select = new();
    private Subject<Unit> _backTitle = new();

    private LoadPresenter _presenter;

    void Start()
    {
        if (PlayerInput.Instance == null)
        {
            Debug.LogError("PlayerInputがnullです。");
            return;
        }
        PlayerInput.Instance.Input.Base.Disable();
        PlayerInput.Instance.Input.LoadScene.Enable();

        _presenter = new LoadPresenter(this);
    }

    public LoadViewOutput Bind()
    {
        // Input
        PlayerInput.Instance.Input.LoadScene.MoveUp.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
            {
                _moveUp.OnNext(Unit.Default);
            }
        };
        PlayerInput.Instance.Input.LoadScene.MoveDown.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
            {
                _moveDown.OnNext(Unit.Default);
            }
        };
        PlayerInput.Instance.Input.LoadScene.Select.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
            {
                _select.OnNext(Unit.Default);
            }
        };
        PlayerInput.Instance.Input.LoadScene.BackTitle.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
            {
                _backTitle.OnNext(Unit.Default);
            }
        };

        LoadViewOutput output = new LoadViewOutput();
        output.moveUp = _moveUp;
        output.moveDown = _moveDown;
        output.select = _select;
        output.backTitle = _backTitle;

        return output;
    }

    public void SetActiveSlot(int index)
    {
        for (int i = 0; i < _loadSlotObjs.Count; i++)
        {
            _loadSlotObjs[i].backgroundImg.sprite = (i == index) ? _activeSlot : _notActiveSlot;
        }
    }

    public void SetSlotItems(List<SlotItem> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (i >= _loadSlotObjs.Count - 1) continue;

            var slotObj = _loadSlotObjs[i];
            slotObj.title.text = items[i].title;
            slotObj.date.text = items[i].date;
        }
    }

    void OnDestroy()
    {
        PlayerInput.Instance.Input.LoadScene.Disable();
        PlayerInput.Instance.Input.Base.Enable();
        _presenter = null;
        Destroy(this.gameObject);
    }
}
