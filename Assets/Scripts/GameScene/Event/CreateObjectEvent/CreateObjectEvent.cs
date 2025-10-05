using UnityEngine;
using R3;

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

    private bool IsFinishEvent()
    {
        return _createdObj != null;
    }

    private bool IsTriggerEvent()
    {
        return _isInEvent && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return));
    }

    public override void TriggerEvent()
    {
        CreateObject();
    }

    public override void OnUpdateEvent()
    {
        // トリガー条件チェック
        if (IsTriggerEvent())
        {
            onTriggerEvent.OnNext(Unit.Default);
        }

        // 終了条件チェック
        if (IsFinishEvent())
        {
            onFinishEvent.OnNext(Unit.Default);
        }
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
