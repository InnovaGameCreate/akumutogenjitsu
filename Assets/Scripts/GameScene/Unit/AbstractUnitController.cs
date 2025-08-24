using UnityEngine;

public abstract class AbstractUnitController : MonoBehaviour
{
    private UnitMove _unitMove;

    // Unitの動きの状態
    private UnitMoveStatus _unitMoveStatus;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _unitMove = GetComponent<UnitMove>();
        if (_unitMove == null)
        {
            Debug.LogError("UnitMoveがアタッチされていません");
        }

        OnStartUnitController();
    }

    void Update()
    {
        OnUpdateUnitController();
        Move();
        _unitMoveStatus = GetMoveStatus(); 
    }

    /// <summary>
    /// UnitControllerの初期化処理(Start()の代わり)
    /// </summary>
    protected virtual void OnStartUnitController()
    {
        return;
    }

    /// <summary>
    /// UnitControllerの更新処理(Update()の代わり)
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
        var moveStatus = GetMoveStatus();
        _unitMove.Move(moveStatus);
    }

    /// <summary>
    /// Unitの移動状態を取得する
    /// </summary>
    public UnitMoveStatus unitMoveStatus
    {
        get { return _unitMoveStatus; }
    }
}
