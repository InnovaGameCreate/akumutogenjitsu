using UnityEngine;

public abstract class AbstractUnitController : MonoBehaviour
{
    private UnitMove _unitMove;

    // Unitの移動状態の情報
    private UnitMoveStatus _unitMoveStatus;

    // Unitの移動有効フラグ
    private bool _enabled = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _unitMove = GetComponent<UnitMove>();
        if (_unitMove == null)
        {
            Debug.LogError("UnitMoveコンポーネントがアタッチされていません");
        }

        OnStartUnitController();
    }

    void Update()
    {
        OnUpdateUnitController();
        // enabledがfalseのときは自動で動かさない
        if (!_enabled) return;

        _unitMoveStatus = GetMoveStatus();
    }

    void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// UnitControllerの初期化処理(Start()の後に実行)
    /// </summary>
    protected virtual void OnStartUnitController()
    {
        return;
    }

    /// <summary>
    /// UnitControllerの更新処理(Update()の後に実行)
    /// </summary>
    protected virtual void OnUpdateUnitController()
    {
        return;
    }

    /// <summary>
    /// Unitの移動状態を取得する
    /// </summary>
    /// <returns> 移動状態 </returns>
    public abstract UnitMoveStatus GetMoveStatus();

    /// <summary>
    /// Unitの移動
    /// </summary>
    protected void Move()
    {
        _unitMove.Move(_unitMoveStatus);
    }

    /// <summary>
    /// Unitの移動状態を取得する
    /// </summary>
    public UnitMoveStatus unitMoveStatus
    {
        get { return _unitMoveStatus; }
        set { _unitMoveStatus = value; }
    }

    /// <summary>
    /// Unitの移動有効フラグ
    /// </summary>
    public bool IsEnabled
    {
        get
        {
            return _enabled;
        }
        set
        {
            _unitMoveStatus.Up = false;
            _unitMoveStatus.Down = false;
            _unitMoveStatus.Left = false;
            _unitMoveStatus.Right = false;
            _enabled = value;
        }
    }
}
