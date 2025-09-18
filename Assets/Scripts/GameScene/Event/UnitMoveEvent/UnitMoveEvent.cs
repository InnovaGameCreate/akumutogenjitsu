using UnityEngine;

public class UnitMoveEvent : AbstractEvent
{
    [Header("動かしたいユニットのタグ")]
    [SerializeField] private string _unitTag;
    [SerializeField] private eDirection _direction;

    [Header("動かす距離")]
    [SerializeField] private float _distance;

    [Header("ユニットの移動速度(指定していないときはオブジェクトの速度を使用する)")]
    [SerializeField] private float _speed;

    [Header("シーンに入ったら自動で実行するか")]
    [SerializeField] private bool _isTriggerForce = false;

    private bool _isInEvent = false;
    private Vector3 _defaultPosition;

    private AbstractUnitController _unitController;
    private UnitMove _unitMove;
    private float _defaultSpeed;

    public override void OnStartEvent()
    {
        GameObject unitObj = GameObject.FindWithTag(_unitTag);
        if (unitObj == null)
        {
            Debug.LogError($"ユニット{_unitTag}を取得することができませんでした。");
        }

        _unitController = unitObj.GetComponent<AbstractUnitController>();
        if (_unitController == null)
        {
            Debug.LogError("AbstractUnitControllerがコンポーネントされていません。");
        }

        _unitMove = unitObj.GetComponent<UnitMove>();
        if (_unitMove == null)
        {
            Debug.LogError("UnitMoveがコンポーネントされていません。");
        }

        _defaultPosition = gameObject.transform.position;
    }
    public override bool IsFinishEvent()
    {
        Vector3 position = _unitMove.gameObject.transform.position;
        switch (_direction)
        {
            case eDirection.Left:
                return (_defaultPosition.x - position.x) >= _distance;

            case eDirection.Right:
                return (position.x - _defaultPosition.x) >= _distance;

            case eDirection.Up:
                return (_defaultPosition.y - position.y) >= _distance;

            case eDirection.Down:
                return (position.y - _defaultPosition.y) >= _distance;

            default:
                Debug.LogError("方向が設定されていません。");
                break;
        }

        return false;
    }

    public override bool IsTriggerEvent()
    {
        return (_isInEvent && Input.GetKeyDown(KeyCode.Z)) || _isTriggerForce;
    }

    public override void TriggerEvent()
    {
        if (_defaultSpeed == 0)
        {
            _defaultSpeed = _unitMove.Speed;
        }
        if (_speed != 0)
        {
            _unitMove.Speed = _speed;
        }

        UnitMoveStatus unitMoveStatus = UnitMoveStatus.CreateMoveStatusFromDirection(_direction);
        _unitController.unitMoveStatus = unitMoveStatus;
    }

    public override void OnFinishEvent()
    {
        if (_speed != 0)
        {
            _unitMove.Speed = _defaultSpeed;
        }
    }

    // MARK: OnTrigger

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInEvent = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInEvent = false;
        }
    }
}
