using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapMoveEvent : AbstractEvent
{
    // ブロックの中にPlayerが入っているか
    private bool _isInEventBlock;

    [Header("移動するマップの名前")]
    [SerializeField] private string _sceneName;

    [Header("移動するマップの座標")]
    [SerializeField] private Vector2 _position;

    [Header("自動でマップを移動するか")]
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
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i); // シーンのパスを取得
            string sceneFileName = System.IO.Path.GetFileNameWithoutExtension(scenePath); // ファイル名のみ取得

            _isScenesExist[sceneFileName] = true;
        }
    }

    public override bool IsTriggerEvent()
    {
        // ブロックの中にPlayerが入っているとき
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
            Debug.LogError($"シーンが存在しません: {_sceneName}");
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
    /// マップを移動する
    /// </summary>
    private void MoveMap()
    {
        // シーンを移動して座標も移動
        _playerMapMove.MapMove(_sceneName, _position);
    }

    /// <summary>
    /// シーンが存在するか
    /// </summary>
    /// <returns> 存在するか </returns>
    private bool IsSceneExist()
    {
        return _isScenesExist.ContainsKey(_sceneName);
    }
}