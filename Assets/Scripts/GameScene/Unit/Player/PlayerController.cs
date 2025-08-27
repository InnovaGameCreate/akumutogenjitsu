using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : AbstractUnitController
{
    private UnitMoveStatus _moveStatus;
    private Vector2 _currentInputVector;

    protected override void OnStartUnitController()
    {
        PlayerInput.Instance.Input.Base.PlayerMove.started += OnMoveInput;
        PlayerInput.Instance.Input.Base.PlayerMove.canceled += OnMoveInput;
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        _currentInputVector = context.ReadValue<Vector2>();
        UpdateMoveStatus();
    }

    protected override void OnUpdateUnitController()
    {
        if (PlayerInput.Instance?.Input?.Base.PlayerMove != null)
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
    }

    public override UnitMoveStatus GetMoveStatus()
    {
        return _moveStatus;
    }
}
