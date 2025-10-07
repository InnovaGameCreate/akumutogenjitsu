using UnityEngine;
using R3;

public class TestEvent : AbstractEvent
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public override void TriggerEvent()
    {
        Debug.Log("イベントを実行");
    }

    public override void OnFinishEvent()
    {
        Debug.Log("イベントを終了");
    }

    public override void OnUpdateEvent()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            // イベントを発生させる
            onTriggerEvent.OnNext(Unit.Default);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            // イベントを終了させる
            onFinishEvent.OnNext(Unit.Default);
        }
    }
}
