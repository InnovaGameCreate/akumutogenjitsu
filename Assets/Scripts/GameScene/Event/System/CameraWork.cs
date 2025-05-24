using UnityEngine;

public class CameraWork : MonoBehaviour
{
    [Header("�J�������Ǐ]����I�u�W�F�N�g")]
    [SerializeField] private GameObject _moveWithObj;

    private Camera _camera;

    private Vector3 _previousPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_moveWithObj == null)
        {
            Debug.Log("Camera���Ǐ]����I�u�W�F�N�g��ݒ肳��Ă��Ȃ��̂ŁAPlayer�^�O�̃I�u�W�F�N�g���擾���܂��B");
            _moveWithObj = GameObject.FindWithTag("Player");

            if (_moveWithObj == null)
            {
                Debug.LogError("Player�^�O�̃I�u�W�F�N�g��������܂���B");
            }
        }

        _camera = GetComponent<Camera>();
        if (_camera == null)
        {
            Debug.LogError("Camera�ɃA�^�b�`����Ă��܂���B");
        }

        _camera.clearFlags = CameraClearFlags.SolidColor;
        _camera.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 1); // �w�i�F�����ɐݒ�
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
