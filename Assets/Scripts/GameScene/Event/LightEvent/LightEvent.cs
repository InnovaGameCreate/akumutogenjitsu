using System.Runtime.CompilerServices;
using R3;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightEvent : AbstractEvent
{
    private enum eConditionType
    {
        Morning,
        Evening,
        Night
    }

    [Header("どの状態にするか？")]
    [SerializeField] private eConditionType _type;

    [Header("夕方用のライト")]
    [SerializeField] private Light2D _eveningLight;
    [Header("夜用のライト")]
    [SerializeField] private Light2D _nightLight;

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
        GameObject playerObj = GameObject.FindWithTag("Player");

        switch (_type)
        {
            case eConditionType.Evening:
                DestroyLight();
                GameObject eveningLight = Instantiate(_eveningLight, playerObj.transform).gameObject;
                DontDestroyOnLoad(eveningLight);
                break;

            case eConditionType.Night:
                DestroyLight();
                GameObject nightLight = Instantiate(_nightLight, playerObj.transform).gameObject;
                DontDestroyOnLoad(nightLight);
                break;

            case eConditionType.Morning:
                DestroyLight();
                break;
        }

        onFinishEvent.OnNext(Unit.Default);
    }

    private void DestroyLight()
    {
        GameObject nightObj = GameObject.FindWithTag("NightLight");
        GameObject eveningObj = GameObject.FindWithTag("EveningLight");
        switch (_type)
        {
            case eConditionType.Evening:
                if (nightObj == null)
                {
                    return;
                }
                Destroy(nightObj);
                break;

            case eConditionType.Night:
                if (eveningObj == null)
                {
                    return;
                }
                Destroy(eveningObj);
                break;

            case eConditionType.Morning:
                if (eveningObj != null)
                {
                    Destroy(eveningObj);
                }
                if (nightObj != null)
                {
                    Destroy(nightObj);
                }
                break;
        }
    }
}
