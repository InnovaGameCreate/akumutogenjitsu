using UnityEngine;

public class EnemyTypeManager : MonoBehaviour
{
    [SerializeField] private eEnemy _enemy;

    /// <summary>
    /// �G�̎��
    /// </summary>
    eEnemy Enemy
    {
        get { return _enemy; }
    }
}
