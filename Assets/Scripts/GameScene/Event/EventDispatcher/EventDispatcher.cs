using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EventDispatcher : MonoBehaviour
{
    [Header("実行するイベント")]
    [SerializeField] private List<GameObject> _eventObjs = new();
    private List<AbstractEvent> _events = new();

    [Header("一度しか実行しない")]
    [SerializeField] private bool _isTriggerOnce;

    [Header("シーンを読み込んだらすぐ実行するか")]
    [SerializeField] private bool _isTriggerForce = false;

    private bool _isInEvent = false;

    private BasicAnimation _animation;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var obj in _eventObjs)
        {
            AbstractEvent evt = obj.GetComponent<AbstractEvent>();
            if (evt == null) continue;

            _events.Add(evt);
        }

        if (_isTriggerForce)
        {
            TriggerAllEvent();
        }

        _animation = gameObject.GetComponent<BasicAnimation>();
        if (_animation == null)
        {
            Debug.LogError("BasicAnimationはアタッチされていません。");
        }

        PlayerInput.Instance.Input.Base.Interact.performed += OnInteract;
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton() && _isInEvent)
        {
            TriggerAllEvent();
        }
    }

    private void TriggerAllEvent()
    {
        foreach (var evt in _events)
        {
            evt.TriggerEventForce();

            if (_isTriggerOnce)
            {
                evt.Enabled = false;
            }
        }

        if (_isTriggerOnce)
        {
            _events.Clear();
            _animation.Enabled = false;
        }
    }

    // OnTrigger
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

    void OnDisable()
    {
        PlayerInput.Instance.Input.Base.Interact.performed -= OnInteract;
    }
}
