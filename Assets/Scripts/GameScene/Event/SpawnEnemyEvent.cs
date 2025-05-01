using UnityEngine;

public class SpawnEnemyEvent : AbstractEvent
{
    [SerializeField] private EnemySpawnManager _enemySpawnManager;

    [Header("スポーン位置")]
    [SerializeField] private Vector2 _position;

    private bool _isInEnter;

    private bool _hasFinished = false;

    public override void TriggerEvent()
    {
        _enemySpawnManager.SpawnEnemy(_position);

        _hasFinished = true;
    }

    public override bool IsTriggerEvent()
    {
        if (EventStatus == eEventStatus.Triggered)
        {
            _hasFinished = false;
        }

        return _isInEnter && (Input.GetKeyDown(KeyCode.Z) || Input.GetKey(KeyCode.Return));
    }

    public override bool IsFinishEvent()
    {
        return _hasFinished;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isInEnter = false;

        if (collision.CompareTag("Player"))
        {
            _isInEnter = true;
        }
    }
}
