using UnityEngine;

public class UnitMove : MonoBehaviour
{
    // 速度
    [SerializeField] private float _defaultSpeed;
    private float _speed;

    // 方向
    private UnitMoveStatus _unitMoveStatus;
    
    // Unitが移動するか
    private bool _isEnabled;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_defaultSpeed == 0)
        {
            Debug.LogError("DefaultSpeedが0に設定されています");
        }
        _speed = _defaultSpeed;
        _isEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Unitを動かす
    /// </summary>
    /// <param name="direction"> 動かす方向 </param>
    public void Move(UnitMoveStatus unitmovestatus)
    {
        if (!_isEnabled)
        {
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
        move = move.normalized * _speed * Time.deltaTime;

        // 移動
        transform.position += (Vector3)move;

        // 方向を取得
        _unitMoveStatus = unitmovestatus;
    }

    /// <summary>
    /// Unitの速度
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
                Debug.LogError("Speedに負の値が設定されています");
                return;
            }

            // 速度を設定
            _speed = value;

            #if DEBUG_MODE
                Debug.Log("Speedを" + _speed + "に設定しました");
            #endif
        }
    }

    /// <summary>
    /// Unitが移動するか
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
