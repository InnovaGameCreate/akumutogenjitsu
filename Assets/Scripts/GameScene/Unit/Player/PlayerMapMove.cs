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

        GameObject eventMgr = GameObject.FindWithTag("EventMgr");
        _eventMgr = eventMgr.GetComponent<EventManager>();
        if (_eventMgr == null)
        {
            Debug.LogError("EventManagerÇ™ë∂ç›ÇµÇ‹ÇπÇÒÅB");
        }
    }

    public void MapMove(string sceneName, Vector2 position)
    {
        _eventMgr.SaveAllEventInScene();
        _newPosition = position;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        transform.position = _newPosition;
        EventManager.Instance.InitializeAllEventInScene();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
