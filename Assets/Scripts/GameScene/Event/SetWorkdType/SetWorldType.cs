using R3;
using UnityEngine;

public class SetWorldType : AbstractEvent
{
    [Header("現実か夢か？")]
    [SerializeField] private eWorldType _worldType;

    public override void TriggerEvent()
    {
        GameObject baseUi = GameObject.FindWithTag("BaseUI");
        BaseUIModel model = baseUi.GetComponent<BaseUIModel>();
        if (model == null)
        {
            Debug.LogError("BaseUIModelがnullです。");
            onFinishEvent.OnNext(Unit.Default);
            return;
        }

        model.SetWorldType(_worldType);

        onFinishEvent.OnNext(Unit.Default);
    }
}
