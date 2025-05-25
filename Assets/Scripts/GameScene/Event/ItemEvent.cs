using UnityEngine;

/// <summary>
/// アイテムイベントを管理するクラス。
/// </summary>
public class ItemEvent : AbstractEvent
{
    [Header("コンポーネントしていなくてもよい(Playerタグのついたオブジェクトが選択される)")]
    [SerializeField] private ItemManager _itemMgr;

    [Header("アイテムの種類")]
    [SerializeField] private eItem _item;

    [Header("アイテムを取得するか")]
    [SerializeField] private bool _isGetItem = true;

    private bool _isInEvent = false;

    private bool _hasFinished = false;

    /// <summary>
    /// 初期化処理を行います。
    /// </summary>
    void Start()
    {
        if (_itemMgr == null)
        {
            _itemMgr = GameObject.FindWithTag("Player").GetComponent<ItemManager>();
            if (_itemMgr == null)
            {
                Debug.LogError("ItemManagerが見つかりません。");
                return;
            }
        }
    }

    /// <summary>
    /// イベントがトリガーされる条件を判定します。
    /// </summary>
    /// <returns>トリガー条件を満たしている場合は true。</returns>
    public override bool IsTriggerEvent()
    {
        return _isInEvent && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return));
    }

    /// <summary>
    /// イベントをトリガーします。
    /// </summary>
    public override void TriggerEvent()
    {
        if (Debug.unityLogger.logEnabled)
        {
            Debug.Log($"アイテム: {_item} を{(_isGetItem ? "取得" : "失う")}しました");
        }
        _itemMgr.SetIsItemOwned(_item, _isGetItem);

#if DEBUG_MODE
        if (_isGetItem)
        {
            Debug.Log($"アイテム: {_item} を取得しました");
        }
        else
        {
            Debug.Log($"アイテム: {_item} を失いました");
        }
#endif

        _hasFinished = true;
    }

    /// <summary>
    /// イベントが終了したかどうかを判定します。
    /// </summary>
    /// <returns>終了している場合は true。</returns>
    public override bool IsFinishEvent()
    {
        return _hasFinished;
    }

    /// <summary>
    /// 2D コライダーに衝突した際の処理を行います。
    /// </summary>
    /// <param name="collision">衝突したコライダー。</param>
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
