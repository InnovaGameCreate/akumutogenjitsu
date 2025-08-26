using R3;
using UnityEngine;

public class InventoryPresenter : MonoBehaviour
{
    [SerializeField] private InventoryModel _model;
    [SerializeField] private InventoryView _view;

    private readonly CompositeDisposable _disposable = new();

    void Start()
    {
        if (_model == null)
        {
            return;
        }
        if (_view == null)
        {
            return;
        }

        _model.Initialize();
        Bind();
    }

    private void Bind()
    {
        // View → Model
        _view.Move
            .Subscribe(direction =>
            {
                _model.MoveSelection(direction);
            })
            .AddTo(_disposable);

        _view.Close
            .Subscribe(_ =>
            {
                _view.ActionMapToInventory(false);
                _view.ReturnToBase();
            })
            .AddTo(_disposable);

        // Model → View
        Observable.CombineLatest(
            _model.OwnedItemDatas,
            _model.SelectedIndex,
            (itemDatas, selectedIndex) => new { ItemDatas = itemDatas, SelectedIndex = selectedIndex }
        )
        .Subscribe(data =>
        {
            _view.UpdateInventory(data.ItemDatas, data.SelectedIndex);
        })
        .AddTo(_disposable);

        // 定期的にアイテムデータを更新
        Observable.Timer(System.TimeSpan.Zero, System.TimeSpan.FromSeconds(0.1f))
            .Subscribe(_ => _model.UpdateItemData())
            .AddTo(_disposable);
    }

    void OnDestroy()
    {
        _disposable.Dispose();
    }
}
