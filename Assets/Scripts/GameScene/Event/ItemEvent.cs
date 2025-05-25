using UnityEngine;

/// <summary>
/// �A�C�e���C�x���g���Ǘ�����N���X�B
/// </summary>
public class ItemEvent : AbstractEvent
{
    [Header("�R���|�[�l���g���Ă��Ȃ��Ă��悢(Player�^�O�̂����I�u�W�F�N�g���I�������)")]
    [SerializeField] private ItemManager _itemMgr;

    [Header("�A�C�e���̎��")]
    [SerializeField] private eItem _item;

    [Header("�A�C�e�����擾���邩")]
    [SerializeField] private bool _isGetItem = true;

    private bool _isInEvent = false;

    private bool _hasFinished = false;

    /// <summary>
    /// �������������s���܂��B
    /// </summary>
    void Start()
    {
        if (_itemMgr == null)
        {
            _itemMgr = GameObject.FindWithTag("Player").GetComponent<ItemManager>();
            if (_itemMgr == null)
            {
                Debug.LogError("ItemManager��������܂���B");
                return;
            }
        }
    }

    /// <summary>
    /// �C�x���g���g���K�[���������𔻒肵�܂��B
    /// </summary>
    /// <returns>�g���K�[�����𖞂����Ă���ꍇ�� true�B</returns>
    public override bool IsTriggerEvent()
    {
        return _isInEvent && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return));
    }

    /// <summary>
    /// �C�x���g���g���K�[���܂��B
    /// </summary>
    public override void TriggerEvent()
    {
        if (Debug.unityLogger.logEnabled)
        {
            Debug.Log($"�A�C�e��: {_item} ��{(_isGetItem ? "�擾" : "����")}���܂���");
        }
        _itemMgr.SetIsItemOwned(_item, _isGetItem);

#if DEBUG_MODE
        if (_isGetItem)
        {
            Debug.Log($"�A�C�e��: {_item} ���擾���܂���");
        }
        else
        {
            Debug.Log($"�A�C�e��: {_item} �������܂���");
        }
#endif

        _hasFinished = true;
    }

    /// <summary>
    /// �C�x���g���I���������ǂ����𔻒肵�܂��B
    /// </summary>
    /// <returns>�I�����Ă���ꍇ�� true�B</returns>
    public override bool IsFinishEvent()
    {
        return _hasFinished;
    }

    /// <summary>
    /// 2D �R���C�_�[�ɏՓ˂����ۂ̏������s���܂��B
    /// </summary>
    /// <param name="collision">�Փ˂����R���C�_�[�B</param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInEvent = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInEvent = false;
        }
    }
}
