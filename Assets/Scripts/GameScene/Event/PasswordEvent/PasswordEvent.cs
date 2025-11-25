using R3;
using UnityEngine;

public class PasswordEvent : AbstractEvent
{
    [Header("パスワード")]
    [SerializeField] private string _correctPassword;

    [Header("正解だった時に起こるイベント")]
    [SerializeField] private GameObject _eventObj;

    private AbstractEvent _event;

    [Header("View")]
    [SerializeField] private PasswordEventView _viewPrefab;
    private PasswordEventPresenter _presenter;

    private GameObject _canvasObj;

    private bool _isInEvent = false;
    public override void OnStartEvent()
    {
        PlayerInput.Instance.OnPerformed(PlayerInput.Instance.Input.Base.Interact)
            .Where(ctx => ctx.ReadValueAsButton() && _isInEvent)
            .Subscribe(_ =>
            {
                onTriggerEvent.OnNext(Unit.Default);
            })
            .AddTo(_disposable);

        _canvasObj = GameObject.FindWithTag("UICanvas");
        if (_canvasObj == null)
        {
            Debug.LogError("UICanvasが存在しません。");
        }
        _event = _eventObj.GetComponent<AbstractEvent>();
        if (_event == null)
        {
            Debug.LogError("AbstractEventがアタッチされていません。");
        }
    }

    public override void TriggerEvent()
    {
        if (_presenter == null)
        {
            if (_canvasObj == null)
            {
                return;
            }
            PlayerInput.Instance.Input.Base.Disable();
            PlayerInput.Instance.Input.PasswordEvent.Enable();
            GameObject viewObj = Instantiate(_viewPrefab.gameObject, _canvasObj.transform);
            _presenter = new PasswordEventPresenter(viewObj.GetComponent<PasswordEventView>(), _correctPassword, _event, 0, this);
        }
    }

    public override void OnFinishEvent()
    {
        PlayerInput.Instance.Input.PasswordEvent.Disable();
        PlayerInput.Instance.Input.Base.Enable();
        _presenter = null;
    }

    // MARK: OnTrigger
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInEvent = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInEvent = false;
        }
    }
}
