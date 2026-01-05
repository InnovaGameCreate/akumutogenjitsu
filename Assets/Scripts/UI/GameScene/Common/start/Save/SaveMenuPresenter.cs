using R3;
using UnityEngine;

public class SaveMenuPresenter : MonoBehaviour
{
    [SerializeField] private SaveMenuModel _model;
    [SerializeField] private SaveMenuView _view;

    private readonly CompositeDisposable _disposal = new();

    void Start()
    {
        _model.Initialize();
        _model.UpdateSaveTitleList();

        Bind();
    }

    private void Bind()
    {
        _view.MoveDown
            .Subscribe(_ =>
            {
                _model.MoveDownSlot();
            })
            .AddTo(_disposal);

        _view.MoveUp
            .Subscribe(_ =>
            {
                _model.MoveUpSlot();
            })
            .AddTo(_disposal);

        _view.Select
            .Subscribe(_ =>
            {
                _model.Save();
            })
            .AddTo(_disposal);

        _view.Close
            .Subscribe(_ =>
            {
                // SaveMenuViewのReturnToMenuメソッドを使用
                _view.ReturnToMenu();
            })
            .AddTo(_disposal);

        _model.ActiveSlotIndex
            .Subscribe(slotIndex => _view.ChangeActiveSlot(slotIndex))
            .AddTo(_disposal);

        _model.SaveTitleList
            .Subscribe(saveList => _view.UpdateSaveList(saveList))
            .AddTo(_disposal);

        // 定期的にセーブリストを更新（外部からのセーブファイル変更に対応）
        Observable.Timer(System.TimeSpan.Zero, System.TimeSpan.FromSeconds(2))
            .Subscribe(_ => _model.UpdateSaveTitleList())
            .AddTo(_disposal);
    }

    void OnDestroy()
    {
        _disposal?.Dispose();
    }
}
