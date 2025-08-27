using UnityEngine;
using R3;

public class UIModel : MonoBehaviour
{
    private readonly ReactiveProperty<bool> _isShowBase = new(true);
    public ReadOnlyReactiveProperty<bool> IsShowBase => _isShowBase;

    private readonly ReactiveProperty<bool> _isShowInventory = new(false);
    public ReadOnlyReactiveProperty<bool> IsShowInventory => _isShowInventory;

    private readonly ReactiveProperty<bool> _isShowMenu = new(false);
    public ReadOnlyReactiveProperty<bool> IsShowMenu => _isShowMenu;

    public void ActiveBase(bool active)
    {
        _isShowBase.Value = active;
    }

    public void ActiveInventory(bool active)
    {
        _isShowInventory.Value = active;
    }

    public void ActiveMenu(bool active)
    {
        _isShowMenu.Value = active;
    }
}
