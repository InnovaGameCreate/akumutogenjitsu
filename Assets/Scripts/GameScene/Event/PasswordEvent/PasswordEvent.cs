using System.Linq;
using R3;
using UnityEngine;

public class PasswordEvent : AbstractEvent
{
    [Header("パスワード")]
    [SerializeField] private string _correctPassword;

    [Header("View")]
    [SerializeField] private GameObject _viewPrefab;
    private PasswordEventPresenter _presenter;

    private GameObject _canvasObj;

    private bool _isInEvent = false;
    public override void OnStartEvent()
    {
        PlayerInput.Instance.Input.Base.Interact.performed += ctx =>
        {
            if (ctx.ReadValueAsButton() && _isInEvent)
            {
                onTriggerEvent.OnNext(Unit.Default);
            }
        };

        _canvasObj = GameObject.FindWithTag("UICanvas");
        if (_canvasObj == null)
        {
            Debug.LogError("UICanvasが存在しません。");
        }
    }

    public override void TriggerEvent()
    {
        if (_presenter == null)
        {
            PlayerInput.Instance.Input.Base.Disable();
            PlayerInput.Instance.Input.PasswordEvent.Enable();
            GameObject viewObj = Instantiate(_viewPrefab, _canvasObj.transform);
            _presenter = new PasswordEventPresenter(viewObj.GetComponent<PasswordEventView>(), _correctPassword, 0);
        }
    }

    public override void OnFinishEvent()
    {
        PlayerInput.Instance.Input.PasswordEvent.Disable();
        PlayerInput.Instance.Input.Base.Enable();
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
