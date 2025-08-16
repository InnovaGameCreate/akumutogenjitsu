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
        Bind();
    }

    private void Bind()
    {
        _view.OnKeyPressed
            .Subscribe(keyCode =>
            {
                switch (keyCode)
                {
                    case KeyCode.UpArrow:
                        _model.MoveUpSlot();
                        break;

                    case KeyCode.DownArrow:
                        _model.MoveDownSlot();
                        break;

                    case KeyCode.Return:
                        Debug.Log($"スロット{_model.ActiveSlotIndex.CurrentValue}が選択されました");
                        break;
                }
            })
        .AddTo(_disposal);

        _model.ActiveSlotIndex
            .Subscribe(slotIndex => _view.ChangeActiveSlot(slotIndex))
            .AddTo(_disposal);
    }

    void OnDestroy()
    {
        _disposal?.Dispose();
    }
}