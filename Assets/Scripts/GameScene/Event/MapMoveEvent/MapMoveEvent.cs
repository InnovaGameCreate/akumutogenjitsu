using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using R3;

public class MapMoveEvent : AbstractEvent
{
    // ブロックの中にPlayerが入っているか
    private bool _isInEventBlock;

    [Header("移動先のマップの名前")]
    [SerializeField] private string _sceneName;

    [Header("移動先のマップの座標")]
    [SerializeField] private Vector2 _position;

    [Header("自動でマップを移動するか")]
    [SerializeField] private bool _isAutoMove;

    private PlayerMapMove _playerMapMove;

    private Dictionary<string, bool> _isScenesExist;

    private bool _hasFinished = false;

    public override void OnStartEvent()
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

    private bool IsTriggerEvent()
    {
        // ブロックの中にPlayerが入っているとき
        if (_isInEventBlock)
        {
            return Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return) || _isAutoMove;
        }

        return false;
    }

    private bool IsFinishEvent()
    {
        if (EventStatus == eEventStatus.Triggered)
        {
            _hasFinished = false;
        }
        return _hasFinished;
    }

    public override void TriggerEvent()
    {
        if (IsSceneExist())
        {
            // 強制的にルートオブジェクトにする
            if (transform.parent != null)
            {
                transform.parent = null;
            }
            // 移動したシーン先でオブジェクトがないと終了処理が実行できないため
            DontDestroyOnLoad(this.gameObject);

            // エネミーにマップ移動を通知（追加部分）
            NotifyEnemyMapMove();

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
        // トリガー条件チェック
        if (IsTriggerEvent())
        {
            _isAutoMove = false;
            onTriggerEvent.OnNext(Unit.Default);
        }

        // 終了条件チェック
        if (IsFinishEvent())
        {
            onFinishEvent.OnNext(Unit.Default);
        }
    }

    public override void OnFinishEvent()
    {
        if (this.gameObject != null)
        {
            // `DontDestroyOnLoad(this.gameObject);`をしているため
            Destroy(this.gameObject);
        }
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
        // シーンを移動して座標を移動
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

    /// <summary>
    /// エネミーにマップ移動を通知（新規追加）
    /// </summary>
    private void NotifyEnemyMapMove()
    {
        // PersistentEnemyManagerが存在する場合、エネミーに移動を通知
        if (PersistentEnemyManager.Instance != null)
        {
            PersistentEnemyManager.Instance.OnPlayerMapMove(_sceneName, _position);
        }
    }
}
