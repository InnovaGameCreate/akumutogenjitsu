using R3;

public class PasswordEventPresenter
{
    private PasswordEvent _passwordEvent;
    private PasswordEventModel _model;
    private PasswordEventView _view;

    private AbstractEvent _nextEvent;

    private CompositeDisposable _disposables = new();

    public PasswordEventPresenter(PasswordEventView view, string password, AbstractEvent nextEvent, int defaultActiveSlot, PasswordEvent passwordEvent)
    {
        _model = new PasswordEventModel(password, defaultActiveSlot);
        _view = view;
        _nextEvent = nextEvent;
        _passwordEvent = passwordEvent;

        Bind();
        _view.CreateSlots(_model.SlotNums.CurrentValue.Count, 0);
    }

    private void Bind()
    {
        PasswordEventViewOutput output = _view.Bind();

        // Input => Model
        output.onMoveLeft
            .Subscribe(_ =>
            {
                _model.MoveLeftSelection();
            })
            .AddTo(_disposables);

        output.onMoveRight
            .Subscribe(_ =>
            {
                _model.MoveRightSelection();
            })
            .AddTo(_disposables);

        output.onPlusNumber
            .Subscribe(_ =>
            {
                _model.PlusNumber();
            })
            .AddTo(_disposables);

        output.onMinusNumber
            .Subscribe(_ =>
            {
                _model.MinusNumber();
            })
            .AddTo(_disposables);

        // 認証
        output.onSubmit
            .Subscribe(_ =>
            {
                _view.DestroyView();
                _passwordEvent.FinishEventForce();
                if (IsCorrectPassword())
                {
                    _nextEvent.TriggerEventForce();
                }
            })
            .AddTo(_disposables);

        // Model => View
        _model.ActiveSlotIndex
            .Subscribe(index =>
            {
                _view.SetActiveSlot(index);
            })
            .AddTo(_disposables);

        _model.SlotNums
            .Subscribe(nums =>
            {
                for (int i = 0; i < nums.Count; i++)
                {
                    _view.SetSlotNums(i, nums[i]);
                }
            })
            .AddTo(_disposables);
    }

    private bool IsCorrectPassword()
    {
        return _model.CorrectPassword.CurrentValue == string.Join("", _model.SlotNums.CurrentValue);
    }

    ~PasswordEventPresenter()
    {
        _disposables?.Dispose();
    }
}
