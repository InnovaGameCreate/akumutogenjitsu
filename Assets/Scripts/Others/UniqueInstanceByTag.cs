using UnityEngine;

/// <summary>
/// コンポーネントされたPrefabのタグを持つオブジェクトが2つ以上存在する場合、Destroyします。
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
                Debug.Log($"オブジェクトが2つ以上存在するのでDestroyしました。削除したオブジェクト: {gameObject.transform.name}");
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
