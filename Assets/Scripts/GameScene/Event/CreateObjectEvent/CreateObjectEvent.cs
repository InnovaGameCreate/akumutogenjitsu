using UnityEngine;

public class CreateObjectEvent : AbstractEvent
{
    [Header("生成するオブジェクトのPrefab")]
    [SerializeField] private GameObject _obj;

    [Header("生成する座標")]
    [SerializeField] private Vector2 _position;

    private GameObject _createdObj;

    private bool _isInEvent = false;

    public override void OnStartEvent()
    {
        if (EventStatus == eEventStatus.Triggered)
        {
            CreateObject();
        }
    }

    public override bool IsFinishEvent()
    {
        return _createdObj != null;
    }

    public override bool IsTriggerEvent()
    {
        return _isInEvent && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return));
    }

    public override void TriggerEvent()
    {
        CreateObject();
    }

    private void CreateObject()
    {
        _createdObj = Instantiate(_obj, _position, Quaternion.identity);
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
