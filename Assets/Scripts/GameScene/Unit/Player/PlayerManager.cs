using UnityEngine;
using VContainer;

public class PlayerManager : MonoBehaviour, ISaveableManager<PlayerSaveData>
{
    [Inject]
    private UnitMove _playerUnitMove;

    private void Awake()
    {
        if (_playerUnitMove == null)
        {
            _playerUnitMove = GetComponent<UnitMove>();
            if (_playerUnitMove == null)
            {
                var playerObj = GameObject.FindWithTag("Player");
                if (playerObj != null)
                {
                    _playerUnitMove = playerObj.GetComponent<UnitMove>();
                }
            }
        }
    }

    private void OnEnable()
    {
        // オブジェクトが有効になったときにUnitMoveの参照を確認
        if (_playerUnitMove == null || _playerUnitMove.gameObject == null)
        {
            TryReacquireUnitMove();
        }
    }

    private void OnDisable()
    {
        // コルーチンが実行中の場合は停止
        StopAllCoroutines();
    }

    public PlayerSaveData EncodeToSaveData()
    {
        if (_playerUnitMove == null || _playerUnitMove.gameObject == null)
        {
            Debug.LogWarning("UnitMoveが無効です。再取得を試行します。");
            TryReacquireUnitMove();
        }

        PlayerSaveData saveData = new PlayerSaveData();
        
        if (_playerUnitMove != null && _playerUnitMove.transform != null)
        {
            saveData.Position = new SerializableVector3(_playerUnitMove.transform.position);
        }
        else
        {
            Debug.LogWarning("PlayerUnitMoveが無効です。デフォルト位置(0,0,0)を保存します。");
            saveData.Position = new SerializableVector3(Vector3.zero);
        }
        
        return saveData;
    }

    public void LoadFromSaveData(PlayerSaveData saveData)
    {
        if (saveData?.Position == null)
        {
            Debug.LogWarning("PlayerSaveDataまたはPositionがnullです。");
            return;
        }

        // シーン遷移直後の場合は少し待ってから実行
        StartCoroutine(DelayedLoadPosition(saveData));
    }

    /// <summary>
    /// 遅延してポジションをロードする
    /// </summary>
    private System.Collections.IEnumerator DelayedLoadPosition(PlayerSaveData saveData)
    {
        yield return new WaitForEndOfFrame();
        yield return null;

        // UnitMoveの再取得を試行
        if (_playerUnitMove == null || _playerUnitMove.gameObject == null)
        {
            Debug.LogWarning("UnitMoveが無効です。再取得を試行します。");
            TryReacquireUnitMove();
        }

        if (_playerUnitMove != null && _playerUnitMove.transform != null)
        {
            _playerUnitMove.transform.position = saveData.Position.ToVector3();
            Debug.Log($"プレイヤー位置を復元しました: {saveData.Position.ToVector3()}");
        }
        else
        {
            Debug.LogError("PlayerUnitMoveが無効です。位置の復元をスキップします。");
        }
    }

    /// <summary>
    /// UnitMoveの再取得を試行
    /// </summary>
    private void TryReacquireUnitMove()
    {
        // まず自分のGameObjectから取得を試行
        _playerUnitMove = GetComponent<UnitMove>();
        
        if (_playerUnitMove == null)
        {
            // PlayerタグのオブジェクトからUnitMoveを取得
            var playerObjects = GameObject.FindGameObjectsWithTag("Player");
            foreach (var playerObj in playerObjects)
            {
                if (playerObj != null && playerObj.activeInHierarchy)
                {
                    var unitMove = playerObj.GetComponent<UnitMove>();
                    if (unitMove != null)
                    {
                        _playerUnitMove = unitMove;
                        break;
                    }
                }
            }
        }
        
        if (_playerUnitMove == null)
        {
            // 最後の手段：シーン内のアクティブなUnitMoveを取得
            var allUnitMoves = FindObjectsByType<UnitMove>(FindObjectsSortMode.None);
            foreach (var unitMove in allUnitMoves)
            {
                if (unitMove != null && unitMove.gameObject.activeInHierarchy)
                {
                    _playerUnitMove = unitMove;
                    break;
                }
            }
        }

        if (_playerUnitMove != null)
        {
            Debug.Log("UnitMoveの再取得に成功しました。");
        }
        else
        {
            Debug.LogError("UnitMoveの再取得に失敗しました。");
        }
    }
}
