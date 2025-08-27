using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : AbstractUnitController
{
    private UnitMoveStatus _moveStatus;
    private Vector2 _currentInputVector;
    private bool _isInputInitialized = false;

    protected override void OnStartUnitController()
    {
        StartCoroutine(InitializeInputWhenReady());
    }

    private IEnumerator InitializeInputWhenReady()
    {
        // PlayerInputが利用可能になるまで待機
        while (PlayerInput.Instance == null || PlayerInput.Instance.Input == null)
        {
            yield return null;
        }

        // PlayerInputが利用可能になったら入力イベントを設定
        try
        {
            PlayerInput.Instance.Input.Base.PlayerMove.started += OnMoveInput;
            PlayerInput.Instance.Input.Base.PlayerMove.canceled += OnMoveInput;
            PlayerInput.Instance.Input.Base.Enable();
            _isInputInitialized = true;
            Debug.Log("PlayerInputの初期化が完了しました。");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"PlayerInputの初期化に失敗しました。 - {e.Message}");
        }
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        _currentInputVector = context.ReadValue<Vector2>();
    }

    protected override void OnUpdateUnitController()
    {
        if (_isInputInitialized && PlayerInput.Instance != null && PlayerInput.Instance.Input != null)
        {
            _currentInputVector = PlayerInput.Instance.Input.Base.PlayerMove.ReadValue<Vector2>();
            UpdateMoveStatus();
        }
    }

    private void UpdateMoveStatus()
    {
        _moveStatus.Up = _currentInputVector.y > 0.1f;
        _moveStatus.Down = _currentInputVector.y < -0.1f;
        _moveStatus.Right = _currentInputVector.x > 0.1f;
        _moveStatus.Left = _currentInputVector.x < -0.1f;

        if (!_moveStatus.Up && !_moveStatus.Down && !_moveStatus.Left && !_moveStatus.Right)
        {
            Debug.Log("Stop");
        }
    }

    public override UnitMoveStatus GetMoveStatus()
    {
        return _moveStatus;
    }

    void OnDestroy()
    {
        // イベントの購読解除
        if (_isInputInitialized && PlayerInput.Instance != null && PlayerInput.Instance.Input != null)
        {
            try
            {
                PlayerInput.Instance.Input.Base.PlayerMove.started -= OnMoveInput;
                PlayerInput.Instance.Input.Base.PlayerMove.canceled -= OnMoveInput;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"{e.Message}");
            }
        }
    }
}
