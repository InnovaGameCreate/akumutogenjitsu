using UnityEngine;

/// <summary>
/// �R���|�[�l���g���ꂽPrefab�̃^�O�����I�u�W�F�N�g��2�ȏ㑶�݂���ꍇ�ADestroy���܂��B
/// </summary>
public class UniqueInstanceByTag : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(gameObject.tag);
        if (objects.Length >= 2)
        {
            Debug.Log($"�I�u�W�F�N�g��2�ȏ㑶�݂���̂�Destroy���܂����B�폜�����I�u�W�F�N�g: {gameObject.transform.name}");
            Destroy(gameObject);
        }
    }
}
