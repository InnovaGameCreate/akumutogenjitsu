using UnityEngine;
using R3;

public class SpawnEnemyEvent : AbstractEvent
{
    [SerializeField] private EnemySpawnManager _enemySpawnManager;

    [Header("スポーン位置")]
    [SerializeField] private Vector2 _position;

    private bool _isInEnter;
    private bool _hasFinished = false;

    /// <summary>
    /// 敵をスポーンさせるイベントを実行します。
    /// </summary>
    public override void TriggerEvent()
    {
        _enemySpawnManager.SpawnEnemy(_position);
        _hasFinished = true;
    }

    /// <summary>
    /// イベントをトリガーする条件を満たしているかを判定します。
    /// </summary>
    /// <returns>条件を満たしている場合は true、それ以外は false。</returns>
    private bool IsTriggerEvent()
    {
        return _isInEnter && (Input.GetKeyDown(KeyCode.Z) || Input.GetKey(KeyCode.Return));
    }

    /// <summary>
    /// イベントが終了したかどうかを判定します。
    /// </summary>
    /// <returns>終了している場合は true、それ以外は false。</returns>
    private bool IsFinishEvent()
    {
        if (EventStatus == eEventStatus.Triggered)
        {
            _hasFinished = false;
        }
        return _hasFinished;
    }

    public override void OnUpdateEvent()
    {
        // トリガー条件チェック
        if (IsTriggerEvent())
        {
            onTriggerEvent.OnNext(Unit.Default);
        }

        // 終了条件チェック
        if (IsFinishEvent())
        {
            onFinishEvent.OnNext(Unit.Default);
        }
    }

    /// <summary>
    /// プレイヤーがトリガー範囲に入った際に呼び出されます。
    /// </summary>
    /// <param name="collision">トリガーに入ったオブジェクトのコライダー。</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInEnter = false;
        }
    }
}
