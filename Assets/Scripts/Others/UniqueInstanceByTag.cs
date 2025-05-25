using UnityEngine;

/// <summary>
/// コンポーネントされたPrefabのタグを持つオブジェクトが2つ以上存在する場合、Destroyします。
/// </summary>
public class UniqueInstanceByTag : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(gameObject.tag);
        if (objects.Length >= 2)
        {
            Debug.Log($"オブジェクトが2つ以上存在するのでDestroyしました。削除したオブジェクト: {gameObject.transform.name}");
            Destroy(gameObject);
        }
    }
}
