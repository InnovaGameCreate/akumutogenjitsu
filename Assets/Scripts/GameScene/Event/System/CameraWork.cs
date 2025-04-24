using UnityEngine;

public class CameraWork : MonoBehaviour
{
    [Header("カメラが追従するオブジェクト")]
    [SerializeField] private GameObject _moveWithObj;

    private Vector3 _previousPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_moveWithObj == null)
        {
            Debug.LogError("Cameraが追従するオブジェクトを設定されていません。");
        }

        if (GetComponent<Camera>() == null)
        {
            Debug.LogError("Cameraにアタッチされていません。");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_moveWithObj == null) return;

        Vector3 currentPosition = _moveWithObj.transform.position;
        if (currentPosition != _previousPosition)
        {
            currentPosition.z = transform.position.z;
            transform.position = currentPosition;
            _previousPosition = currentPosition;
        }
    }
}
