using UnityEngine;

public class SpawnEnemyEvent : AbstractEvent
{
    [SerializeField] private EnemySpawnManager _enemySpawnManager;

    [Header("�X�|�[���ʒu")]
    [SerializeField] private Vector2 _position;

    private bool _isInEnter;
    private bool _hasFinished = false;

    /// <summary>
    /// �G���X�|�[��������C�x���g�����s���܂��B
    /// </summary>
    public override void TriggerEvent()
    {
        _enemySpawnManager.SpawnEnemy(_position);
        _hasFinished = true;
    }

    /// <summary>
    /// �C�x���g���g���K�[��������𖞂����Ă��邩�𔻒肵�܂��B
    /// </summary>
    /// <returns>�����𖞂����Ă���ꍇ�� true�A����ȊO�� false�B</returns>
    public override bool IsTriggerEvent()
    {
        if (EventStatus == eEventStatus.Triggered)
        {
            _hasFinished = false;
        }

        return _isInEnter && (Input.GetKeyDown(KeyCode.Z) || Input.GetKey(KeyCode.Return));
    }

    /// <summary>
    /// �C�x���g���I���������ǂ����𔻒肵�܂��B
    /// </summary>
    /// <returns>�I�����Ă���ꍇ�� true�A����ȊO�� false�B</returns>
    public override bool IsFinishEvent()
    {
        return _hasFinished;
    }

    /// <summary>
    /// �v���C���[���g���K�[�͈͂ɓ������ۂɌĂяo����܂��B
    /// </summary>
    /// <param name="collision">�g���K�[�ɓ������I�u�W�F�N�g�̃R���C�_�[�B</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isInEnter = false;

        if (collision.CompareTag("Player"))
        {
            _isInEnter = true;
        }
    }
}
