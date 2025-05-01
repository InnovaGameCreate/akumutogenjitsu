using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    // “G‚ÌPrefab
    [SerializeField] private GameObject _enemyPrefab;
    private List<GameObject> _spawnedEnemies = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_enemyPrefab == null)
        {
            Debug.LogError($"{gameObject.name} ‚Ì EnemySpawner ‚É EnemyPrefab ‚ªŠ„‚è“–‚Ä‚ç‚ê‚Ä‚¢‚Ü‚¹‚ñB");
            return;
        }

        SpawnEnemy(new Vector2(0, 0)); // ‰ŠúˆÊ’u‚É“G‚ğ¶¬
    }

    /// <summary>
    /// Enemy‚ğ¶¬‚·‚é
    /// </summary>
    /// <param name="position"> À•W </param>
    public void SpawnEnemy(Vector2 position)
    {
        GameObject enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
        _spawnedEnemies.Add(enemy);
    }

    /// <summary>
    /// ¶¬‚³‚ê‚½‘S‚Ä‚Ì“G‚ğíœ‚·‚é
    /// </summary>
    public void DestroyAllEnemies()
    {
        foreach (var enemy in _spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        _spawnedEnemies.Clear();
    }
}
