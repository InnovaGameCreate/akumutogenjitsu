using UnityEngine;

public class TestEvent : AbstractEvent
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public override bool IsTriggerEvent()
    {
        return Input.GetKey(KeyCode.Space);
    }

    public override void TriggerEvent()
    {
#if DEBUG_MODE
        Debug.Log($"イベント: {Event} がトリガーされました");
#endif
    }

    public override bool IsFinishEvent()
    {
        return Input.GetKeyDown(KeyCode.A);
    }

    public override void OnUpdateEvent()
    {
    }
}
