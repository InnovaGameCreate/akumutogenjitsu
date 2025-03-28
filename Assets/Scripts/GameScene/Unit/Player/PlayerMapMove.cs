using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMapMove : MonoBehaviour
{
    private Vector2 _newPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // マップ移動したときに破壊されないようにする
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// マップの移動
    /// </summary>
    /// <param name="sceneName"> シーン名 </param>
    /// <param name="position"> 座標 </param>
    public void MapMove(string sceneName, Vector2 position)
    {
        _newPosition = position;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // プレイヤーの座標を移動する
        transform.position = _newPosition;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
