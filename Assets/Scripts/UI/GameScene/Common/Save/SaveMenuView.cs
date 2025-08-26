using System;
using System.Collections.Generic;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SaveSlotView
{
    [SerializeField] public Image SlotBg;
    [SerializeField] public TextMeshProUGUI Date;
    [SerializeField] public TextMeshProUGUI Title;
}

public class SaveMenuView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private List<SaveSlotView> _slots;

    [Header("Sprite")]
    [SerializeField] private Sprite _activeSlotBg;
    [SerializeField] private Sprite _inactiveSlotBg;

    private readonly Subject<Unit> _moveUp = new();
    public Observable<Unit> MoveUp => _moveUp;

    private readonly Subject<Unit> _moveDown = new();
    public Observable<Unit> MoveDown => _moveDown;

    private readonly Subject<Unit> _select = new();
    public Observable<Unit> Select => _select;

    private readonly Subject<Unit> _close = new();
    public Observable<Unit> Close => _close;

    private UIInput _uiInput;
    private MenuView _menuView;

    void Awake()
    {
        _uiInput = new UIInput();
    }

    void Start()
    {
        _menuView = FindFirstObjectByType<MenuView>();

        BindToInput();
    }

    void OnEnable()
    {
        // SaveMenuが表示された時にActionMapを有効にする
        if (_uiInput != null)
        {
            ActionMapToSave(true);
        }
    }

    void OnDisable()
    {
        // SaveMenuが非表示になった時にActionMapを無効にする
        if (_uiInput != null)
        {
            ActionMapToSave(false);
        }
    }

    public void BindToInput()
    {
        _uiInput.Save.MoveToUp.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
            {
                OnMoveUpInput();
            }
        };

        _uiInput.Save.MoveToDown.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
            {
                OnMoveDownInput();
            }
        };

        _uiInput.Save.Select.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
            {
                OnSelectInput();
            }
        };

        _uiInput.Save.Close.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
            {
                OnCloseInput();
            }
        };
    }

    // MARK: Show
    public void ChangeActiveSlot(int slot)
    {
        if (slot < 0 || slot >= _slots.Count)
        {
            Debug.LogError("スロットは0～2にしてください。");
            return;
        }

        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i]?.SlotBg != null)
                _slots[i].SlotBg.sprite = i == slot ? _activeSlotBg : _inactiveSlotBg;
        }
    }

    public void UpdateSaveList(List<SaveList> saveLists)
    {
        if (saveLists == null || _slots == null) return;

        int minCount = Mathf.Min(saveLists.Count, _slots.Count);

        for (int i = 0; i < minCount; ++i)
        {
            if (_slots[i]?.Date != null)
                _slots[i].Date.text = saveLists[i].Date;

            if (_slots[i]?.Title != null)
                _slots[i].Title.text = saveLists[i].Title;
        }
    }

    public void ReturnToMenu()
    {
        if (_menuView != null)
        {
            _menuView.ShowSaveMenu(false);
            ActionMapToSave(false);
            _menuView.ActionMapToMenu(true);
        }
    }

    // MARK: Input
    public void OnMoveUpInput()
    {
        _moveUp?.OnNext(Unit.Default);
    }

    public void OnMoveDownInput()
    {
        _moveDown?.OnNext(Unit.Default);
    }

    public void OnSelectInput()
    {
        _select?.OnNext(Unit.Default);
    }

    public void OnCloseInput()
    {
        _close?.OnNext(Unit.Default);
    }

    // MARK: ActionMap
    public void ActionMapToSave(bool active)
    {
        if (active)
        {
            _uiInput.Save.Enable();
        }
        else
        {
            _uiInput.Save.Disable();
        }
    }

    public void ActionMapToMenu(bool active)
    {
        if (active)
        {
            _uiInput.Menu.Enable();
        }
        else
        {
            _uiInput.Menu.Disable();
        }
    }

    void OnDestroy()
    {
        _uiInput?.Dispose();
        _moveUp?.Dispose();
        _moveDown?.Dispose();
        _select?.Dispose();
        _close?.Dispose();
    }
}
