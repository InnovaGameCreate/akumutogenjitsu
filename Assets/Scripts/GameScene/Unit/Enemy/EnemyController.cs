using UnityEngine;
using UnityEngine.Rendering;

public class EnemyController : AbstractUnitController
{
    // PlayerObject
    private GameObject _playerObj;

    private eDirection _direction;

    [Header("1•à‚ğ•à‚­ŠÔ")]
    [SerializeField] private float _stepLength = 1.0f;
    private float _deltaTime;   // Œo‚Á‚½ŠÔ

    // “®‚­•ûŒü
    private UnitMoveStatus _unitMoveStatus;

    protected override void OnStartUnitController()
    {
        _playerObj = GameObject.FindGameObjectWithTag("Player");
        _direction = eDirection.Left; // ‰Šú•ûŒü‚ğ¶‚Éİ’è

#if DEBUG_MODE
        if (_playerObj == null)
        {
            Debug.LogError("Player‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ");
        }
#endif

        _deltaTime = 0;

        // UnitMoveStatus‚Ì‰Šú‰»
        _unitMoveStatus.Right = false;
        _unitMoveStatus.Left = false;
        _unitMoveStatus.Up = false;
        _unitMoveStatus.Down = false;
    }

    public override UnitMoveStatus GetMoveStatus()
    {
        if (_deltaTime < _stepLength)
        {
            _deltaTime += Time.deltaTime;
            return _unitMoveStatus;
        }
        _deltaTime = 0;

        _unitMoveStatus.Left = false;
        _unitMoveStatus.Right = false;
        _unitMoveStatus.Up = false;
        _unitMoveStatus.Down = false;

        if (_direction == eDirection.Up || _direction == eDirection.Down)
        {
            if (_playerObj.transform.position.x < transform.position.x)
            {
                _unitMoveStatus.Left = true;
                _direction = eDirection.Left;
            }
            else
            {
                _unitMoveStatus.Right = true;
                _direction = eDirection.Right;
            }
        }
        else if (_direction == eDirection.Left || _direction == eDirection.Right)
        {
            if (_playerObj.transform.position.y < transform.position.y)
            {
                _unitMoveStatus.Down = true;
                _direction = eDirection.Down;
            }
            else
            {
                _unitMoveStatus.Up = true;
                _direction = eDirection.Up;
            }
        }
#if DEBUG_MODE
        else
        {
            Debug.LogError($"–³Œø‚È•ûŒü‚ªw’è‚³‚ê‚Ä‚¢‚Ü‚·Bdirection: {_direction}");
        }
#endif

        return _unitMoveStatus;
    }
}
