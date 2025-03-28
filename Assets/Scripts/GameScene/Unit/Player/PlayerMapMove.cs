using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMapMove : MonoBehaviour
{
    private Vector2 _newPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �}�b�v�ړ������Ƃ��ɔj�󂳂�Ȃ��悤�ɂ���
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// �}�b�v�̈ړ�
    /// </summary>
    /// <param name="sceneName"> �V�[���� </param>
    /// <param name="position"> ���W </param>
    public void MapMove(string sceneName, Vector2 position)
    {
        _newPosition = position;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �v���C���[�̍��W���ړ�����
        transform.position = _newPosition;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
