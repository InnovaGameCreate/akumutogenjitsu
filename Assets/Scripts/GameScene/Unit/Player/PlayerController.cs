using UnityEngine;

public class PlayerController : AbstractUnitController
{
    public override UnitMoveStatus GetMoveStatus()
    {
        UnitMoveStatus status;

        status.Left = Input.GetKey(KeyCode.LeftArrow);
        status.Right = Input.GetKey(KeyCode.RightArrow);
        status.Up = Input.GetKey(KeyCode.UpArrow);
        status.Down = Input.GetKey(KeyCode.DownArrow);

        return status;
    }
}
