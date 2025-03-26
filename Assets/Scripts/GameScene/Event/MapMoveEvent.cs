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

    private void Start()
    {
        _isInEventBlock = false;
    }

    public override bool IsTriggerEvent()
    {
        // ブロックの中にPlayerが入っているとき
        if (_isInEventBlock)
        {
            return Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return);
        }

        return false;
    }

    public override bool IsFinishEvent()
    {
        // Eventが実行中でかつブロックの中にPlayerが入っていないとき
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
            Debug.LogError($"シーンが存在しません: {_sceneName}");
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
    /// マップを移動する
    /// </summary>
    private void MoveMap()
    {
        // シーンを移動して座標も移動
        PlayerMapMove player = GameObject.FindWithTag("Player").GetComponent<PlayerMapMove>();
        player.MapMove(_sceneName, _position);
    }

    /// <summary>
    /// シーンが存在するか
    /// </summary>
    /// <returns> 存在するか </returns>
    private bool IsSceneExist()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i); // シーンのパスを取得
            string sceneFileName = System.IO.Path.GetFileNameWithoutExtension(scenePath); // ファイル名のみ取得

            if (sceneFileName == _sceneName) // 完全一致で比較
            {
                return true;
            }
        }

        return false;
    }
}