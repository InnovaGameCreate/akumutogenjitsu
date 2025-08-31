using UnityEngine;

public class UnitMove : MonoBehaviour
{
    // 速度の初期値
    [SerializeField] private float _defaultSpeed;
    private float _speed;

    // 移動入力状態
    private UnitMoveStatus _unitMoveStatus;

    // Unitの移動有効フラグ
    private bool _isEnabled;

    private Rigidbody2D _rigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_defaultSpeed == 0)
        {
            Debug.LogError("DefaultSpeedが0に設定されています。");
        }

        _rigidbody = GetComponent<Rigidbody2D>();
        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody2Dがアタッチされていません。");
        }

        _speed = _defaultSpeed;
        _isEnabled = true;
    }

    /// <summary>
    /// ユニットを移動させる
    /// </summary>
    /// <param name="direction"> 移動方向の入力状態 </param>
    public void Move(UnitMoveStatus unitmovestatus)
    {
        if (!_isEnabled)
        {
            Debug.Log("UnitMoveが無効化されています。移動できません。");
            return;
        }

        Vector2 move = Vector2.zero;

        if (unitmovestatus.Left)
        {
            move.x = -1;
        }
        if (unitmovestatus.Right)
        {
            move.x = 1;
        }
        if (unitmovestatus.Up)
        {
            move.y = 1;
        }
        if (unitmovestatus.Down)
        {
            move.y = -1;
        }

        move = move.normalized * _speed * Time.fixedDeltaTime;

    // 移動実行
        _rigidbody.MovePosition(_rigidbody.position + move);

    // 入力状態を保持
        _unitMoveStatus = unitmovestatus;
    }

    /// <summary>
    /// ユニットの速度
    /// </summary>
    public float Speed
    {
        get
        {
            return _speed;
        }

        set
        {
            if (value < 0)
            {
                Debug.LogError("Speedに負の値を設定することはできません。");
                return;
            }

            // 速度を設定
            _speed = value;

#if DEBUG_MODE
            Debug.Log("Speedが" + _speed + "に設定されました");
#endif
        }
    }

    /// <summary>
    /// ユニットの移動有効フラグ
    /// </summary>
    public bool IsEnabled
    {
        get
        {
            return _isEnabled;
        }
        set
        {
            _isEnabled = value;
            enabled = _isEnabled;
        }
    }
}
