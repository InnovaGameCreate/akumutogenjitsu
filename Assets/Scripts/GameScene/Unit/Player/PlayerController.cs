using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : AbstractUnitController
{
    private UnitMoveStatus _moveStatus;

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();

        _moveStatus.Up = move.y > 0;
        _moveStatus.Down = move.y < 0;
        _moveStatus.Right = move.x > 0;
        _moveStatus.Left = move.x < 0;
    }

    public override UnitMoveStatus GetMoveStatus()
    {
        return _moveStatus;
    }
}
