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
            Debug.LogError("ItemManagerが見つかりません。");
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
            Debug.Log("すでに玄関の鍵を持っている。");
        }

        if (_itemMgr.GetIsItemOwned(eItem.MedicineBlue) && _itemMgr.GetIsItemOwned(eItem.MedicineRed))
        {
            _itemMgr.SetIsItemOwned(eItem.EntranceKey, true);

            _itemMgr.SetIsItemOwned(eItem.MedicineBlue, false);
            _itemMgr.SetIsItemOwned(eItem.MedicineRed, false);
            Debug.Log("薬品を組み合わせると玄関の鍵ができた。");
            Debug.Log("できた。この鍵なら開くはず...！");
        }
        else
        {
            Debug.Log("まだなにか足りない気がする...");
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
