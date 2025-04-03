using UnityEngine;

public class EnemyDamagePlayer : MonoBehaviour
{
    private GameObject _playerObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerObj = GameObject.FindGameObjectWithTag("Player");
#if DEBUG_MODE
        if (_playerObj == null)
        {
            Debug.LogError("Player��������܂���");
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DamagePlayer();
        }
    }

    /// <summary>
    /// Player���U��
    /// </summary>
    private void DamagePlayer()
    {
#if DEBUG_MODE

        Debug.Log("Player�ɐڐG���܂����B");
#endif
    }
}

