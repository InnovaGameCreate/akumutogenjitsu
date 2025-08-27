using UnityEngine;
using R3;

public class BaseUIPresenter : MonoBehaviour
{
    [SerializeField] private BaseUIModel _model;
    [SerializeField] private BaseUIView _view;

    private readonly CompositeDisposable _disposable = new();

    void Start()
    {
        if (_model == null)
        {
            Debug.LogError("BaseUIModelがアサインされていません。");
            return;
        }
        if (_view == null)
        {
            Debug.LogError("BaseUIViewがアサインされていません。");
            return;
        }

        Bind();
    }

    private void Bind()
    {
        // Model → View のバインディング
        
        // 日付の変更を監視
        _model.CurrentDate
            .Subscribe(date => _view.UpdateDateText(date.Month, date.Day))
            .AddTo(_disposable);

        // 場所の変更を監視
        _model.CurrentPlace
            .Subscribe(place => _view.UpdatePlaceText(place))
            .AddTo(_disposable);

        // ワールドタイプの変更を監視
        _model.WorldType
            .Subscribe(worldType => _view.UpdateWorldTypeDisplay(worldType))
            .AddTo(_disposable);
    }

    // 外部からワールドタイプを変更するためのパブリックメソッド
    public void SetWorldType(eWorldType worldType)
    {
        _model.SetWorldType(worldType);
    }

    // 手動更新を強制
    public void ForceUpdate()
    {
        _model.ForceUpdate();
    }

    // 現在の値を取得するためのメソッド
    public Date GetCurrentDate()
    {
        return _model.CurrentDate.CurrentValue;
    }

    public string GetCurrentPlace()
    {
        return _model.CurrentPlace.CurrentValue;
    }

    public eWorldType GetCurrentWorldType()
    {
        return _model.WorldType.CurrentValue;
    }

    void OnDestroy()
    {
        _disposable.Dispose();
    }
}
