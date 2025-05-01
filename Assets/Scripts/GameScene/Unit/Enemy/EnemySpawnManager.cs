using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    // �G��Prefab
    [SerializeField] private GameObject _enemyPrefab;
    private List<GameObject> _spawnedEnemies = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_enemyPrefab == null)
        {
            Debug.LogError($"{gameObject.name} �� EnemySpawner �� EnemyPrefab �����蓖�Ă��Ă��܂���B");
            return;
        }

        SpawnEnemy(new Vector2(0, 0)); // �����ʒu�ɓG�𐶐�
    }

    /// <summary>
    /// Enemy�𐶐�����
    /// </summary>
    /// <param name="position"> ���W </param>
    public void SpawnEnemy(Vector2 position)
    {
        GameObject enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
        _spawnedEnemies.Add(enemy);
    }

    /// <summary>
    /// �������ꂽ�S�Ă̓G���폜����
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
