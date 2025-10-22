using R3;
using UnityEngine;

public class SeEvent : AbstractEvent
{
    [Header("SE")]
    [SerializeField] private AudioData _seAudioData;
    [SerializeField] private bool _isTriggerForce = false;
    private bool _isInEvent;
    public override void OnStartEvent()
    {
        PlayerInput.Instance.Input.Base.Interact.performed += ctx =>
        {
            if (ctx.ReadValueAsButton() && _isInEvent)
            {
                onTriggerEvent.OnNext(Unit.Default);
            }
        };

        if (_isTriggerForce)
        {
            onTriggerEvent.OnNext(Unit.Default);
        }
    }

    public override void TriggerEvent()
    {
        Audio.Instance.PlaySe(_seAudioData);
        onFinishEvent.OnNext(Unit.Default);
    }

    // MARK: OnTrigger

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInEvent = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInEvent = false;
        }
    }
}
