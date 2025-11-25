using R3;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EveningEvent : AbstractEvent
{
    [Header("夕方にするか？(チェックボックスをつけない場合ライトを消す)")]
    [SerializeField] private bool _isAppear = true;

    [Header("夕方用のライト")]
    [SerializeField] private Light2D _eveningLight;

    [Header("シーンに入った直後イベントを実行するか")]
    [SerializeField] private bool _isTriggeredOnce = false;

    public override void OnStartEvent()
    {
        if (_isTriggeredOnce)
        {
            onTriggerEvent.OnNext(Unit.Default);
        }
    }

    public override void TriggerEvent()
    {
        if (_isAppear)
        {
            GameObject light2d = Instantiate(_eveningLight).gameObject;
            DontDestroyOnLoad(light2d);
        }
        else
        {
            GameObject light2d = GameObject.FindWithTag("EveningLight");
            Destroy(light2d);
        }

        onFinishEvent.OnNext(Unit.Default);
    }
}
