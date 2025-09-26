using UnityEngine;

/// <summary>
/// アイテムに関する処理を行うイベントクラス
/// </summary>
public class ItemEvent : AbstractEvent
{
    [Header("未設定の場合 (Playerの子からItemManagerを探します)")]
    [SerializeField] private ItemManager _itemMgr;

    [Header("対象のアイテム")]
    [SerializeField] private eItem _item;

    [Header("アイテムを取得するか")]
    [SerializeField] private bool _isGetItem = true;

    private bool _isInEvent = false;

    private bool _hasFinished = false;

    /// <summary>
    /// 初期化処理を行います
    /// </summary>
    public override void OnStartEvent()
    {
        if (_itemMgr == null)
        {
            _itemMgr = GameObject.FindWithTag("Player").GetComponentInChildren<ItemManager>();
            if (_itemMgr == null)
            {
                Debug.LogError("ItemManagerが見つかりませんでした。");
                return;
            }
        }
    }

    /// <summary>
    /// イベントのトリガー条件を判定します
    /// </summary>
    /// <returns>トリガー条件を満たす場合は true</returns>
    public override bool IsTriggerEvent()
    {
        return _isInEvent && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return));
    }

    /// <summary>
    /// イベントを実行します
    /// </summary>
    public override void TriggerEvent()
    {
        if (Debug.unityLogger.logEnabled)
        {
            Debug.Log($"アイテム: {_item} を{(_isGetItem ? "入手" : "喪失")}しました");
        }
        _itemMgr.SetIsItemOwned(_item, _isGetItem);

#if DEBUG_MODE
        if (_isGetItem)
        {
            Debug.Log($"アイテム: {_item} を入手しました");
        }
        else
        {
            Debug.Log($"アイテム: {_item} を喪失しました");
        }
#endif

        _hasFinished = true;
    }

    /// <summary>
    /// イベントの終了判定を行います
    /// </summary>
    /// <returns>終了した場合は true</returns>
    public override bool IsFinishEvent()
    {
        return _hasFinished;
    }

    /// <summary>
    /// 2D のトリガーに入った際の処理を行います
    /// </summary>
    /// <param name="collision">衝突したコライダー</param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInEvent = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInEvent = false;
        }
    }
}
