using UnityEngine;

class SetDateEvent : AbstractEvent
{
    [Header("シーンに入ったらすぐに実行するか")]
    [SerializeField] private bool _isTriggerForce = false;

    private bool _isInEvent = false;

    private bool _hasFinished = false;

    public override void TriggerEvent()
    {
        Date currentDate = DateManager.Instance.GetCurrentDate();
        DateManager.Instance.SetCurrentDate(Date.AddDays(currentDate, 1));
        _hasFinished = true;
    }

    public override bool IsFinishEvent()
    {
        if (_hasFinished)
        {
            _hasFinished = false;
            return true;
        }
        return false;
    }

    public override bool IsTriggerEvent()
    {
        return (_isInEvent && Input.GetKeyDown(KeyCode.Z)) || _isTriggerForce;
    }

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
