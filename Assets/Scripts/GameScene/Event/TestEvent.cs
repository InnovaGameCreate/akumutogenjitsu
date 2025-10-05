using UnityEngine;
using R3;

public class TestEvent : AbstractEvent
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public override void TriggerEvent()
    {
    }

    public override void OnFinishEvent()
    {
    }

    public override void OnUpdateEvent()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            onTriggerEvent.OnNext(Unit.Default);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            onFinishEvent.OnNext(Unit.Default);
        }
    }
}
