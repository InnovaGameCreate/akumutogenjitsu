using UnityEngine;

public class EnemyEvent : AbstractEvent
{
    [SerializeField] private EnemySpawnManager _enemySpawnManager;

    private bool _isInEnter = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TriggerEvent()
    {
        _enemySpawnManager.SpawnEnemy(new Vector2(0, 0)); // ìGÇê∂ê¨
    }

    public override bool IsTriggerEvent()
    {
        return _isInEnter || (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.Return));
    }

    public override bool IsFinishEvent()
    {
        return _isInEnter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInEnter = true;
        }
    }
}
