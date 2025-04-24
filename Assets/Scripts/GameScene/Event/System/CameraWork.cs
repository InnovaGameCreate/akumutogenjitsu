using UnityEngine;

public class CameraWork : MonoBehaviour
{
    [Header("�J�������Ǐ]����I�u�W�F�N�g")]
    [SerializeField] private GameObject _moveWithObj;

    private Vector3 _previousPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_moveWithObj == null)
        {
            Debug.LogError("Camera���Ǐ]����I�u�W�F�N�g��ݒ肳��Ă��܂���B");
        }

        if (GetComponent<Camera>() == null)
        {
            Debug.LogError("Camera�ɃA�^�b�`����Ă��܂���B");
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
