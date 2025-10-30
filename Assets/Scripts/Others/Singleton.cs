using UnityEngine;

/// <summary>
/// MonoBehaviour用のシングルトンベースクラス
/// </summary>
/// <typeparam name="T">シングルトンとして扱うMonoBehaviourクラス</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                Debug.Log($"[Singleton] アプリケーション終了時にインスタンス '{typeof(T)}' はすでに破棄されています。再生成せず、nullを返します。");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();

                    if (_instance == null)
                    {
                        Debug.Log($"[Singleton] シーンに {typeof(T)} のインスタンスが必要ですが、存在しません。");
                    }
                    else
                    {
                        if (_instance.transform.parent == null)
                        {
                            DontDestroyOnLoad(_instance.gameObject);
                        }
                    }
                }

                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else if (_instance != this)
        {
            Debug.Log($"[Singleton] 他のインスタンス[{typeof(T)}]がすでに存在していますので、このオブジェクトを削除します。");
            Destroy(gameObject);
        }
    }

    protected virtual void OnApplicationQuit()
    {
        _applicationIsQuitting = true;
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
