using UnityEngine;

public class CreateEntranceKeyWithMedicineEvent : AbstractEvent
{
    private ItemManager _itemMgr;

    private bool _isPlayerIn = false;

    private bool _hasFinished = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnStartEvent()
    {
        _itemMgr = GameObject.FindWithTag("ItemMgr").GetComponent<ItemManager>();
        if (_itemMgr == null)
        {
            Debug.LogError("ItemManagerが見つかりませんでした。");
        }
    }

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
        if (_itemMgr.GetIsItemOwned(eItem.EntranceKey))
        {
            Debug.Log("既に玄関の鍵を所持しています。");
        }

        if (_itemMgr.GetIsItemOwned(eItem.MedicineBlue) && _itemMgr.GetIsItemOwned(eItem.MedicineRed))
        {
            _itemMgr.SetIsItemOwned(eItem.EntranceKey, true);

            _itemMgr.SetIsItemOwned(eItem.MedicineBlue, false);
            _itemMgr.SetIsItemOwned(eItem.MedicineRed, false);
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
