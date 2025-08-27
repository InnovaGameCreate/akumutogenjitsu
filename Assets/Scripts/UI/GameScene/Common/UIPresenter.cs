using UnityEngine;
using R3;

public class UIPresenter : MonoBehaviour
{
    [SerializeField] private UIModel _model;
    [SerializeField] private UIView _view;

    private readonly CompositeDisposable _disposable = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Bind();
    }

    private void Bind()
    {
        _view.BaseInput
            .Subscribe(_ =>
            {
                if (_model.IsShowBase.CurrentValue)
                {
                    _model.ActiveBase(false);
                }
                else
                {
                    _model.ActiveBase(true);
                }
            })
            .AddTo(_disposable);

        _view.InventoryInput
            .Subscribe(_ =>
            {
                if (_model.IsShowInventory.CurrentValue)
                {
                    _model.ActiveInventory(false);
                }
                else
                {
                    _model.ActiveInventory(true);
                }
            })
            .AddTo(_disposable);

        _view.MenuInput
            .Subscribe(_ =>
            {
                if (_model.IsShowMenu.CurrentValue)
                {
                    _model.ActiveMenu(false);
                }
                else
                {
                    _model.ActiveMenu(true);
                }
            })
            .AddTo(_disposable);

        _model.IsShowBase
            .Subscribe(active =>
            {
                _view.ShowBase(active);
                _view.ActionMapToBase(active);
            })
            .AddTo(_disposable);

        _model.IsShowInventory
            .Subscribe(active =>
            {
                _view.ShowInventory(active);
                _view.ActionMapToBase(!active);
                _view.ActionMapToInventory(active);
            })
            .AddTo(_disposable);

        _model.IsShowMenu
            .Subscribe(active =>
            {
                _view.ShowMenu(active);
                _view.ActionMapToBase(!active);
                _view.ActionMapToMenu(active);
            })
            .AddTo(_disposable);
    }

    void OnDestroy()
    {
        _disposable?.Dispose();
    }
}
