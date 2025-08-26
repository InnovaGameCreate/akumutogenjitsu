using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : AbstractUnitController
{
    private UnitMoveStatus _moveStatus;
    private Vector2 _currentMoveInput;

    private PlayerOperation _playerOperation;

    protected override void OnStartUnitController()
    {
        _playerOperation = new PlayerOperation();
        _playerOperation.Base.PlayerMove.started += ctx => OnMove(ctx);
        _playerOperation.Base.PlayerMove.performed += ctx => OnMove(ctx);
        _playerOperation.Base.PlayerMove.canceled += ctx => OnMove(ctx);
        _playerOperation.Base.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _currentMoveInput = context.ReadValue<Vector2>();

        _moveStatus.Up = _currentMoveInput.y > 0;
        _moveStatus.Down = _currentMoveInput.y < 0;
        _moveStatus.Right = _currentMoveInput.x > 0;
        _moveStatus.Left = _currentMoveInput.x < 0;
    }

    public override UnitMoveStatus GetMoveStatus()
    {
        return _moveStatus;
    }

    void OnDestroy()
    {
        _playerOperation?.Dispose();
    }
}
