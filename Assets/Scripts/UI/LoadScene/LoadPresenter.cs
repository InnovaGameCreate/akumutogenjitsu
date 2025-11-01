using R3;
using UnityEngine.SceneManagement;

class LoadPresenter
{
    private readonly LoadView _view;
    private readonly LoadModel _model;

    private readonly CompositeDisposable _disposables = new();

    public LoadPresenter(LoadView view)
    {
        _view = view;
        _model = new LoadModel(0);

        Bind();
    }

    private void Bind()
    {
        // Input -> Model
        LoadViewOutput viewOutput = _view.Bind();
        viewOutput.moveUp
            .Subscribe(_ =>
            {
                _model.MoveUp();
            })
            .AddTo(_disposables);
        viewOutput.moveDown
            .Subscribe(_ =>
            {
                _model.MoveDown();
            })
            .AddTo(_disposables);
        viewOutput.select
            .Subscribe(_ =>
            {
                // セーブデータのロード
                _model.LoadSaveData();
            })
            .AddTo(_disposables);

        // Model -> View
        LoadModelOutput modelOutput = _model.Bind();
        modelOutput.slotIndex
            .Subscribe(index =>
            {
                _view.SetActiveSlot(index);
            })
            .AddTo(_disposables);
        modelOutput.slotItems
            .Subscribe(items =>
            {
                _view.SetSlotItems(items);
            })
            .AddTo(_disposables);
    }

    ~LoadPresenter()
    {
        _disposables?.Dispose();
    }
}
