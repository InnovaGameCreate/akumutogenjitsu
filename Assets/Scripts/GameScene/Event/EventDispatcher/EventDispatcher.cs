using System.Collections.Generic;
using NUnit.Framework;
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
            if (obj == null)
            {
                Debug.LogWarning("[EventDispatcher] イベントオブジェクトがnullです。");
                continue;
            }

            AbstractEvent evt = obj.GetComponent<AbstractEvent>();
            if (evt == null)
            {
                Debug.LogWarning($"[EventDispatcher] {obj.name} にAbstractEventがアタッチされていません。");
                continue;
            }

            _events.Add(evt);
        }

        if (_isTriggerForce)
        {
            TriggerAllEvent();
        }

        _animation = gameObject.GetComponent<BasicAnimation>();
        if (_animation == null)
        {
            Debug.LogWarning("[EventDispatcher] BasicAnimationがアタッチされていません。アニメーション機能は無効です。");
        }

        if (PlayerInput.Instance == null)
        {
            Debug.LogError("[EventDispatcher] PlayerInputが存在しません。");
            return;
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
            if (evt == null)
            {
                Debug.LogWarning("[EventDispatcher] イベントがnullです。");
                continue;
            }

            evt.TriggerEventForce();

            if (_isTriggerOnce)
            {
                evt.Enabled = false;
            }
        }

        if (_isTriggerOnce)
        {
            _events.Clear();
            if (_animation != null)
            {
                _animation.Enabled = false;
            }
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
        if (PlayerInput.Instance != null)
        {
            PlayerInput.Instance.Input.Base.Interact.performed -= OnInteract;
        }
    }
}
