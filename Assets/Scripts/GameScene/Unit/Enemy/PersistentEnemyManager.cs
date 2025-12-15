using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// エネミーをシーン間で永続化し、プレイヤーのマップ移動に追従させる
/// </summary>
public class PersistentEnemyManager : MonoBehaviour
{
    private static PersistentEnemyManager _instance;

    [Header("追跡エネミーのプレハブ")]
    [SerializeField] private GameObject _enemyPrefab;

    [Header("シーン移動後の出現遅延時間(秒)")]
    [SerializeField] private float _spawnDelay = 3.0f;

    private GameObject _currentEnemy;
    private EnemyController _enemyController;
    private bool _isWaitingToSpawn = false;
    private Vector2 _targetSpawnPosition;
    private string _targetSceneName;

    [Header("エネミー追跡状態")]
    [SerializeField] private bool _isChasing = false;

    [Header("出現確率（分母）")]
    [SerializeField] private int _spawnChance = 20; // 1/20 = 5%

    [Header("逃げ切り確率（分母）")]
    [SerializeField] private int _escapeChance = 20; // 1/20 = 5%

    void Awake()
    {
        // シングルトンパターン
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // シーンロードイベントを購読
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// プレイヤーがマップ移動する際に呼び出す
    /// </summary>
    /// <param name="sceneName">移動先のシーン名</param>
    /// <param name="position">移動先の座標</param>
    public void OnPlayerMapMove(string sceneName, Vector2 position)
    {
        // 追跡中の場合、逃げ切り判定
        if (_isChasing)
        {
            int escapeRoll = Random.Range(1, _escapeChance + 1);
            if (escapeRoll == 1) // 1/20の確率で逃げ切り
            {
#if DEBUG_MODE
                Debug.Log($"エネミーから逃げ切りました！ ({escapeRoll}/{_escapeChance})");
#endif
                StopChasing();
                return; // 追跡終了
            }
#if DEBUG_MODE
            else
            {
                Debug.Log($"逃げ切れませんでした... ({escapeRoll}/{_escapeChance})");
            }
#endif
        }

        // まだ追跡していない場合、出現確率判定
        if (!_isChasing)
        {
            int spawnRoll = Random.Range(1, _spawnChance + 1);
            if (spawnRoll == 1) // 1/20の確率
            {
                _isChasing = true;
#if DEBUG_MODE
                Debug.Log($"エネミーが出現します！ ({spawnRoll}/{_spawnChance})");
#endif
            }
            else
            {
#if DEBUG_MODE
                Debug.Log($"エネミーは出現しませんでした ({spawnRoll}/{_spawnChance})");
#endif
                return; // 出現しない
            }
        }

        _targetSceneName = sceneName;
        _targetSpawnPosition = position;
        _isWaitingToSpawn = true;

        // エネミーを一時的に非表示にする
        if (_currentEnemy != null)
        {
            _currentEnemy.SetActive(false);

            // エネミーコントローラーを停止
            if (_enemyController != null)
            {
                _enemyController.IsEnabled = false;
            }
        }

#if DEBUG_MODE
        Debug.Log($"エネミー追跡継続: シーン={sceneName}, 座標={position}");
#endif
    }

    /// <summary>
    /// シーンがロードされた時の処理
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_isWaitingToSpawn && scene.name == _targetSceneName)
        {
            StartCoroutine(DelayedSpawn());
        }
    }

    /// <summary>
    /// 遅延してエネミーを出現させる
    /// </summary>
    private IEnumerator DelayedSpawn()
    {
        // 指定秒数待機
        yield return new WaitForSeconds(_spawnDelay);

        // エネミーが存在しない場合は生成
        if (_currentEnemy == null)
        {
            SpawnEnemy(_targetSpawnPosition);
        }
        else
        {
            // エネミーの座標を設定して再表示
            _currentEnemy.transform.position = _targetSpawnPosition;
            _currentEnemy.SetActive(true);

            // エネミーコントローラーを再開
            if (_enemyController != null)
            {
                _enemyController.IsEnabled = true;
            }
        }

        _isWaitingToSpawn = false;

#if DEBUG_MODE
        Debug.Log($"エネミーが出現しました: 座標={_targetSpawnPosition}");
#endif
    }

    /// <summary>
    /// エネミーを生成する
    /// </summary>
    private void SpawnEnemy(Vector2 position)
    {
        if (_enemyPrefab == null)
        {
            Debug.LogError("エネミープレハブが設定されていません");
            return;
        }

        _currentEnemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
        _enemyController = _currentEnemy.GetComponent<EnemyController>();

        if (_enemyController == null)
        {
            Debug.LogError("エネミープレハブにEnemyControllerがアタッチされていません");
        }

        // エネミーもDontDestroyOnLoadにする
        DontDestroyOnLoad(_currentEnemy);

#if DEBUG_MODE
        Debug.Log($"エネミーを生成しました: 座標={position}");
#endif
    }

    /// <summary>
    /// 現在のエネミーインスタンスを取得
    /// </summary>
    public GameObject CurrentEnemy => _currentEnemy;

    /// <summary>
    /// 追跡状態を取得
    /// </summary>
    public bool IsChasing => _isChasing;

    /// <summary>
    /// 追跡を開始する（外部から呼び出し可能）
    /// </summary>
    public void StartChasing()
    {
        _isChasing = true;
#if DEBUG_MODE
        Debug.Log("エネミーの追跡が開始されました（手動）");
#endif
    }

    /// <summary>
    /// 追跡を停止する（外部から呼び出し可能）
    /// </summary>
    public void StopChasing()
    {
        _isChasing = false;

        // エネミーを削除
        if (_currentEnemy != null)
        {
            Destroy(_currentEnemy);
            _currentEnemy = null;
            _enemyController = null;
        }

#if DEBUG_MODE
        Debug.Log("エネミーの追跡が停止されました");
#endif
    }

    /// <summary>
    /// シングルトンインスタンス
    /// </summary>
    public static PersistentEnemyManager Instance => _instance;
}
