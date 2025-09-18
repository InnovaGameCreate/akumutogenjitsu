/// <summary>
/// UnitÇÃà⁄ìÆèÛë‘Çï\Ç∑
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

    public static UnitMoveStatus CreateMoveStatusFromDirection(eDirection direction)
    {
        UnitMoveStatus moveStatus = CreateMoveStatus(false, false, false, false);
        switch (direction)
        {
            case eDirection.Left:
                moveStatus.Left = true; break;

            case eDirection.Right:
                moveStatus.Right = true; break;

            case eDirection.Down:
                moveStatus.Down = true; break;

            case eDirection.Up:
                moveStatus.Up = true; break;

            default:
                break;
        }

        return moveStatus;
    }
}
