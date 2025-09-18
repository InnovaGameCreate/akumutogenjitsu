using UnityEngine;

public class UnitMoveEvent : AbstractEvent
{
    [Header("動かしたいユニットのタグ")]
    [SerializeField] private string _unitTag;

    [Header("動かしたい場所")]
    [SerializeField] private Vector2 _destination;

    [Header("ユニットの移動速度(指定していないときはオブジェクトの速度を使用する)")]
    [SerializeField] private float _speed;

    [Header("シーンに入ったら自動で実行するか")]
    [SerializeField] private bool _isTriggerForce = false;

    private bool _isInEvent = false;

    private UnitMove _unitMove = new();

    public override void OnStartEvent()
    {
        GameObject unitObj = GameObject.FindWithTag(_unitTag);
        if (unitObj == null)
        {
            Debug.LogError($"ユニット{_unitTag}を取得することができませんでした。");
        }

        _unitMove = unitObj.GetComponent<UnitMove>();
    }
    public override bool IsFinishEvent()
    {
        throw new System.NotImplementedException();
    }

    public override bool IsTriggerEvent()
    {
        return (_isInEvent && Input.GetKeyDown(KeyCode.Z)) || _isTriggerForce;
    }

    public override void TriggerEvent()
    {
        throw new System.NotImplementedException();
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
