using UnityEngine;

public class CreateEntranceKeyWithMedicineEvent : AbstractEvent
{
    private bool _isPlayerIn = false;

    private bool _hasFinished = false;

    public override bool IsFinishEvent()
    {
        if (EventStatus == eEventStatus.Triggered)
        {
            _hasFinished = false;
        }
        return _hasFinished;
    }

    public override bool IsTriggerEvent()
    {
        return _isPlayerIn && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return));
    }

    public override void TriggerEvent()
    {
        if (ItemManager.Instance.GetIsItemOwned(eItem.EntranceKey))
        {
            Debug.Log("既に玄関の鍵を所持しています。");
        }

        if (ItemManager.Instance.GetIsItemOwned(eItem.MedicineBlue) && ItemManager.Instance.GetIsItemOwned(eItem.MedicineRed))
        {
            ItemManager.Instance.SetIsItemOwned(eItem.EntranceKey, true);

            ItemManager.Instance.SetIsItemOwned(eItem.MedicineBlue, false);
            ItemManager.Instance.SetIsItemOwned(eItem.MedicineRed, false);
            Debug.Log("薬を組み合わせて玄関の鍵を作成しました。");
            Debug.Log("使用した薬は消費されました。");
        }
        else
        {
            Debug.Log("必要な薬が揃っていません。");
        }
        _hasFinished = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isPlayerIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isPlayerIn = false;
        }
    }
}
