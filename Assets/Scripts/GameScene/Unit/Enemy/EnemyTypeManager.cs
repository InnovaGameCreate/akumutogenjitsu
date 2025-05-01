using UnityEngine;

public class EnemyTypeManager : MonoBehaviour
{
    [SerializeField] private eEnemy _enemy;

    /// <summary>
    /// “G‚ÌŽí—Þ
    /// </summary>
    eEnemy Enemy
    {
        get { return _enemy; }
    }
}
