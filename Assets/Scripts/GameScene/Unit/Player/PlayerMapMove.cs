using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMapMove : MonoBehaviour
{
    private Vector2 _newPosition;

    private EventManager _eventMgr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _eventMgr = GameObject.FindGameObjectWithTag("EventMgr").GetComponent<EventManager>();
        if (_eventMgr == null)
        {
            Debug.LogError("EventManager��������܂���ł����B");
        }
    }

    /// <summary>
    //  �}�b�v���ړ�����
    /// </summary>
    /// <param name="sceneName"> �V�[���� </param>
    /// <param name="position"> �ړ���̍��W </param>
    public void MapMove(string sceneName, Vector2 position)
    {
        _eventMgr.SaveAllEventInScene();
        _newPosition = position;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// �V�[�������[�h��������
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        transform.position = _newPosition;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
