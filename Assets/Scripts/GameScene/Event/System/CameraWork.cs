using UnityEngine;

public class CameraWork : MonoBehaviour
{
    [Header("カメラが追従するオブジェクト")]
    [SerializeField] private GameObject _moveWithObj;

    private Camera _camera;

    private Vector3 _previousPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_moveWithObj == null)
        {
            Debug.Log("Cameraが追従するオブジェクトが設定されていないので、Playerタグのオブジェクトを取得します。");
            _moveWithObj = GameObject.FindWithTag("Player");

            if (_moveWithObj == null)
            {
                Debug.LogError("Playerタグのオブジェクトが見つかりません。");
            }
        }

        _camera = GetComponent<Camera>();
        if (_camera == null)
        {
            Debug.LogError("Cameraにアタッチされていません。");
        }

        _camera.clearFlags = CameraClearFlags.SolidColor;
        _camera.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 1); // 背景色を暗く設定
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
