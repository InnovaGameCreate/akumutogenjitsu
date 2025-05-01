using UnityEngine;

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
    public override bool IsTriggerEvent()
    {
        if (EventStatus == eEventStatus.Triggered)
        {
            _hasFinished = false;
        }

        return _isInEnter && (Input.GetKeyDown(KeyCode.Z) || Input.GetKey(KeyCode.Return));
    }

    /// <summary>
    /// イベントが終了したかどうかを判定します。
    /// </summary>
    /// <returns>終了している場合は true、それ以外は false。</returns>
    public override bool IsFinishEvent()
    {
        return _hasFinished;
    }

    /// <summary>
    /// プレイヤーがトリガー範囲に入った際に呼び出されます。
    /// </summary>
    /// <param name="collision">トリガーに入ったオブジェクトのコライダー。</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isInEnter = false;

        if (collision.CompareTag("Player"))
        {
            _isInEnter = true;
        }
    }
}
