using UnityEngine;

public class DeleteObjectEvent : AbstractEvent
{
    [Header("削除するオブジェクト")]
    [SerializeField] private GameObject _obj;

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

    public override bool IsFinishEvent()
    {
        return !_obj.activeInHierarchy;
    }

    public override bool IsTriggerEvent()
    {
        return _isInEvent && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return));
    }

    public override void TriggerEvent()
    {
        if (Enabled)
        {
            _obj.SetActive(false);
            Enabled = false;
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
