using UnityEngine;
using R3;

public class DeleteObjectEvent : AbstractEvent
{
    [Header("削除するオブジェクト")]
    [SerializeField] private GameObject _obj;

    [SerializeField] private bool _isTriggerForce = false;

    private bool _isInEvent = false;

    public override void OnStartEvent()
    {
        if (_obj == null)
        {
            Debug.LogError("_objが存在しません。");
        }
        if (!Enabled)
        {
            _obj.SetActive(false);
        }
    }

    private bool IsFinishEvent()
    {
        return !_obj.activeInHierarchy;
    }

    private bool IsTriggerEvent()
    {
        return _isInEvent && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)) || _isTriggerForce;
    }

    public override void TriggerEvent()
    {
        if (Enabled)
        {
            _obj.SetActive(false);
            Enabled = false;
        }
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
