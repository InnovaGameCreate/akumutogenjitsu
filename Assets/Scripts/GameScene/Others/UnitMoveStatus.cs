using R3;

/// <summary>
/// Unit�̈ړ����
/// </summary>
public struct UnitMoveStatus
{
    public bool Left;
    public bool Right;
    public bool Up;
    public bool Down;

    public static UnitMoveStatus CreateMoveStatus(bool up, bool down, bool right, bool left)
    {
        UnitMoveStatus moveStatus = new UnitMoveStatus();
        moveStatus.Up = up;
        moveStatus.Down = down;
        moveStatus.Right = right;
        moveStatus.Left = left;

        return moveStatus;
    }
}