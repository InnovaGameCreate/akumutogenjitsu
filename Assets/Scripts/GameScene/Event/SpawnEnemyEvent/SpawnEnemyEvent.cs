using UnityEngine;
using R3;

public class SpawnEnemyEvent : AbstractEvent
{
    [SerializeField] private EnemyController _enemyPrefab;

    [Header("スポーン位置")]
    [SerializeField] private Vector2 _position;

    private bool _isInEnter;
    private bool _hasFinished = false;

    /// <summary>
    /// 敵をスポーンさせるイベントを実行します。
    /// </summary>
    public override void TriggerEvent()
    {
        if (_enemyPrefab == null)
        {
            Debug.LogError("_enemyPrefabがnullです。");
        }
        else
        {
            Instantiate(_enemyPrefab, _position, transform.rotation);
        }
        onFinishEvent.OnNext(Unit.Default);
    }

    public override void OnUpdateEvent()
    {
        if (_isInEnter && (Input.GetKeyDown(KeyCode.Z) || Input.GetKey(KeyCode.Return)))
        {
            onTriggerEvent.OnNext(Unit.Default);
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
