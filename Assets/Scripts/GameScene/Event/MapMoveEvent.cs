using UnityEngine;

public class MapMoveEvent : AbstractEvent
{
    // ブロックの中にPlayerが入っているか
    private bool _isInEventBlock;

    [Header("移動するマップの名前")]
    [SerializeField] private string _sceneName;

    [Header("移動するマップの座標")]
    [SerializeField] private Vector2 _position;

    private void Start()
    {
        _isInEventBlock = false;
    }

    public override bool IsTriggerEvent()
    {
        if (_isInEventBlock)
        {
            return Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return);
        }

        return false;
    }

    public override bool IsFinishEvent()
    {
        return EventStatus == eEventStatus.Running && !_isInEventBlock;
    }

    public override void TriggerEvent()
    {
        Debug.Log("Move Map");
    }

    public override void OnUpdateEvent()
    {
#if DEBUG_MODE
        Debug.Log($"_isInEventBlock: {_isInEventBlock}, EventStatus: {EventStatus}");
#endif
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isInEventBlock = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isInEventBlock = false;
        }
    }
}

