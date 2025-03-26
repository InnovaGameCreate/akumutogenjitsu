using UnityEngine;
using UnityEngine.SceneManagement;

public class MapMoveEvent : AbstractEvent
{
    // �u���b�N�̒���Player�������Ă��邩
    private bool _isInEventBlock;

    [Header("�ړ�����}�b�v�̖��O")]
    [SerializeField] private string _sceneName;

    [Header("�ړ�����}�b�v�̍��W")]
    [SerializeField] private Vector2 _position;

    private void Start()
    {
        _isInEventBlock = false;
    }

    public override bool IsTriggerEvent()
    {
        // �u���b�N�̒���Player�������Ă���Ƃ�
        if (_isInEventBlock)
        {
            return Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return);
        }

        return false;
    }

    public override bool IsFinishEvent()
    {
        // Event�����s���ł��u���b�N�̒���Player�������Ă��Ȃ��Ƃ�
        return EventStatus == eEventStatus.Running && !_isInEventBlock;
    }

    public override void TriggerEvent()
    {
        if (IsSceneExist())
        {
            MoveMap();
        }
        else
        {
            Debug.LogError($"�V�[�������݂��܂���: {_sceneName}");
        }
    }

    public override void OnUpdateEvent()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isInEventBlock = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isInEventBlock = false;
        }
    }

    /// <summary>
    /// �}�b�v���ړ�����
    /// </summary>
    private void MoveMap()
    {
        // �V�[�����ړ����č��W���ړ�
        PlayerMapMove player = GameObject.FindWithTag("Player").GetComponent<PlayerMapMove>();
        player.MapMove(_sceneName, _position);
    }

    /// <summary>
    /// �V�[�������݂��邩
    /// </summary>
    /// <returns> ���݂��邩 </returns>
    private bool IsSceneExist()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i); // �V�[���̃p�X���擾
            string sceneFileName = System.IO.Path.GetFileNameWithoutExtension(scenePath); // �t�@�C�����̂ݎ擾

            if (sceneFileName == _sceneName) // ���S��v�Ŕ�r
            {
                return true;
            }
        }

        return false;
    }
}