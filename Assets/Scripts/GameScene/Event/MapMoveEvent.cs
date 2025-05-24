using System.Collections.Generic;
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

    [Header("�����Ń}�b�v���ړ����邩")]
    [SerializeField] private bool _isAutoMove;

    private PlayerMapMove _playerMapMove;

    private Dictionary<string, bool> _isScenesExist;

    private bool _hasFinished = false;

    private void Start()
    {
        _isInEventBlock = false;
        _playerMapMove = GameObject.FindWithTag("Player").GetComponent<PlayerMapMove>();

        _isScenesExist = new Dictionary<string, bool>();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i); // �V�[���̃p�X���擾
            string sceneFileName = System.IO.Path.GetFileNameWithoutExtension(scenePath); // �t�@�C�����̂ݎ擾

            _isScenesExist[sceneFileName] = true;
        }
    }

    public override bool IsTriggerEvent()
    {
        // �u���b�N�̒���Player�������Ă���Ƃ�
        if (_isInEventBlock)
        {
            return Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return) || _isAutoMove;
        }

        return false;
    }

    public override bool IsFinishEvent()
    {
        _hasFinished = (EventStatus == eEventStatus.Triggered && _hasFinished == true) ? false : _hasFinished;
        return _hasFinished;
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
        _hasFinished = true;
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
        _playerMapMove.MapMove(_sceneName, _position);
    }

    /// <summary>
    /// �V�[�������݂��邩
    /// </summary>
    /// <returns> ���݂��邩 </returns>
    private bool IsSceneExist()
    {
        return _isScenesExist.ContainsKey(_sceneName);
    }
}