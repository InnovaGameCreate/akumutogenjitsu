using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    // 敵のPrefab
    [SerializeField] private EnemyTypeManager _enemyTypeMgr;
    private List<GameObject> _spawnedEnemies = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_enemyTypeMgr == null)
        {
            Debug.LogError($"{gameObject.name} の EnemySpawner に EnemyPrefab が割り当てられていません。");
            return;
        }
    }

    /// <summary>
    /// Enemyを生成する
    /// </summary>
    /// <param name="position"> 座標 </param>
    public void SpawnEnemy(Vector2 position)
    {
        GameObject enemy = Instantiate(_enemyTypeMgr.gameObject, position, Quaternion.identity);
        _spawnedEnemies.Add(enemy);
    }

    /// <summary>
    /// 生成された全ての敵を削除する
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
