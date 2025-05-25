using UnityEngine;

/// <summary>
/// �R���|�[�l���g���ꂽPrefab�̃^�O�����I�u�W�F�N�g��2�ȏ㑶�݂���ꍇ�ADestroy���܂��B
/// </summary>
public class UniqueInstanceByTag : MonoBehaviour
{
    private bool _isMainObj = false;

    private void Start()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(gameObject.tag);
        if (objects.Length == 1)
        {
            _isMainObj = true;
            DontDestroyOnLoad(gameObject);
            return;
        }
        foreach (GameObject obj in objects)
        {
            if (!obj.GetComponent<UniqueInstanceByTag>().IsMainObj)
            {
                Debug.Log($"�I�u�W�F�N�g��2�ȏ㑶�݂���̂�Destroy���܂����B�폜�����I�u�W�F�N�g: {gameObject.transform.name}");
                Destroy(obj);
            }
        }
    }

    public bool IsMainObj
    {
        get
        {
            return _isMainObj;
        }
    }
}
