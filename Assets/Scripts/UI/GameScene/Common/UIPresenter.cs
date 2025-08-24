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
        _view.OnShowBase
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

        _view.OnShowInventory
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

        _view.OnShowMenu
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

        _view.OnShowSaveMenu
            .Subscribe(_ =>
            {
                if (_model.IsShowSaveMenu.CurrentValue)
                {
                    _model.ActiveSaveMenu(false);
                    _model.ActiveMenu(true);
                }
                else
                {
                    _model.ActiveSaveMenu(true);
                    _model.ActiveMenu(false);
                }
            })
            .AddTo(_disposable);

        _model.IsShowBase
            .Subscribe(active => _view.ShowBase(active))
            .AddTo(_disposable);

        _model.IsShowInventory
            .Subscribe(active => _view.ShowInventory(active))
            .AddTo(_disposable);

        _model.IsShowMenu
            .Subscribe(active => _view.ShowMenu(active))
            .AddTo(_disposable);

        _model.IsShowSaveMenu
            .Subscribe(active => _view.ShowSaveMenu(active))
            .AddTo(_disposable);
    }

    void OnDestroy()
    {
        _disposable?.Dispose();
    }
}
