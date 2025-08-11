using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour, ISaveableManager<EventSaveData>
{
    // ロードされたEventData
    private Dictionary<string, EventData> _loadedEventDatas = new();

    [SerializeField] private StoryManager _storyManager;

    void Start()
    {
        if (_storyManager == null)
        {
            _storyManager = GameObject.FindGameObjectWithTag("StoryMgr").GetComponent<StoryManager>();
            if (_storyManager == null)
            {
                Debug.LogError("StoryManagerが指定されていません。");
            }
        }

        // 既にロード済みのイベントデータがある場合、シーン内のイベントに適用
        if (_loadedEventDatas.Count > 0)
        {
            // 少し待ってからイベントデータを適用（他のオブジェクトの初期化を待つ）
            StartCoroutine(DelayedApplyEventData());
        }
    }

    /// <summary>
    /// 遅延してイベントデータを適用
    /// </summary>
    private System.Collections.IEnumerator DelayedApplyEventData()
    {
        yield return new UnityEngine.WaitForEndOfFrame();
        yield return null; // 1フレーム待機
        
        ApplyLoadedDataToSceneEvents();
    }

    void OnEnable()
    {
        // シーン遷移後にオブジェクトが有効になったときにもイベントデータを適用
        if (_loadedEventDatas.Count > 0)
        {
            // StartCoroutineが使えない場合に備えて、次フレームで実行
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    void OnDisable()
    {
        // イベント登録解除
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        StopAllCoroutines();
    }

    /// <summary>
    /// シーンがロードされた時の処理
    /// </summary>
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // 新しいシーンのイベントを初期化
        InitializeNewSceneEvents();
        
        if (_loadedEventDatas.Count > 0)
        {
            StartCoroutine(DelayedApplyEventData());
        }
    }

    /// <summary>
    /// 新しいシーンのイベントを初期化（セーブデータに存在しないイベントを追加）
    /// </summary>
    private void InitializeNewSceneEvents()
    {
        GameObject[] eventObjects = GameObject.FindGameObjectsWithTag("Event");
        
        foreach (GameObject obj in eventObjects)
        {
            AbstractEvent evt = obj.GetComponent<AbstractEvent>();
            if (evt == null) continue;

            if (!HasLoadedEvent(evt.EventId))
            {
                // セーブデータに存在しない新しいイベントを初期状態で追加
                SaveEventData(evt.EventId, evt.EventData);
            }
        }
    }

    /// <summary>
    /// シーン上のイベントのEventDataをセーブする
    /// </summary>
    public void SaveAllEventInScene()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Event");
        
        foreach (GameObject obj in objs)
        {
            AbstractEvent evt = obj.GetComponent<AbstractEvent>();
            if (evt == null)
            {
                Debug.LogError($"イベントがコンポーネントされていません。(Event: {obj.name})");
                continue;
            }

            SaveEventData(evt.EventId, evt.EventData);
        }
    }

    /// <summary>
    /// EventDataを保存する
    /// </summary>
    /// <param name="eventId"> EventID </param>
    /// <param name="eventData"> 保存するデータ </param>
    public void SaveEventData(string eventId, EventData eventData)
    {
        if (HasLoadedEvent(eventId))
        {
            _loadedEventDatas[eventId] = eventData;
        }
        else
        {
            _loadedEventDatas.Add(eventId, eventData);
        }
    }

    /// <summary>
    /// イベントがロードされているか
    /// </summary>
    /// <param name="eventId"> イベントID </param>
    /// <returns> 読み込まれているか </returns>
    public bool HasLoadedEvent(string eventId)
    {
        return _loadedEventDatas.ContainsKey(eventId);
    }

    /// <summary>
    /// EventDataを取得する
    /// </summary>
    /// <param name="eventId">EventID </param>
    /// <returns> データ </returns>
    public EventData LoadEventData(string eventId)
    {
        if (_loadedEventDatas.TryGetValue(eventId, out EventData eventData))
        {
            return eventData;
        }
        else
        {
            Debug.LogError($"EventData not found for ID: {eventId}");
            return default(EventData);
        }
    }

    /// <summary>
    /// シーン上のイベントのEventDataをロードする（既存メソッド、下位互換性のため保持）
    /// </summary>
    public void LoadAllEventInScene()
    {
        ApplyLoadedDataToSceneEvents();
    }

    /// <summary>
    /// セーブ済みのEventData(Read Only)
    /// </summary>
    public Dictionary<string, EventData> EventDatas
    {
        get
        {
            return _loadedEventDatas;
        }
    }

    /// <summary>
    /// イベント関連のセーブ・ロード機能
    /// </summary>

    public EventSaveData EncodeToSaveData()
    {
        // セーブ前にシーン内のすべてのイベントを自動保存
        SaveAllEventInScene();
        
        EventSaveData saveData = new EventSaveData();
        
        if (_storyManager != null)
        {
            saveData.CurrentStoryLayer = _storyManager.CurrentStoryLayer;
        }
        
        saveData.EventData = _loadedEventDatas;
        
        return saveData;
    }

    public void LoadFromSaveData(EventSaveData saveData)
    {
        if (saveData == null)
        {
            Debug.LogError("EventSaveData is null");
            return;
        }

        if (_storyManager != null)
        {
            _storyManager.CurrentStoryLayer = saveData.CurrentStoryLayer;
        }
        
        _loadedEventDatas = saveData.EventData ?? new Dictionary<string, EventData>();
        
        // セーブデータからロード後、シーン内のイベントに反映
        ApplyLoadedDataToSceneEvents();
    }

    /// <summary>
    /// ロード済みのEventDataをシーン内のイベントに反映する
    /// </summary>
    public void ApplyLoadedDataToSceneEvents()
    {
        GameObject[] eventObjects = GameObject.FindGameObjectsWithTag("Event");
        int appliedCount = 0;
        
        foreach (GameObject obj in eventObjects)
        {
            AbstractEvent evt = obj.GetComponent<AbstractEvent>();
            if (evt == null)
            {
                Debug.LogWarning($"EventタグのオブジェクトにAbstractEventコンポーネントがありません: {obj.name}");
                continue;
            }

            if (HasLoadedEvent(evt.EventId))
            {
                EventData loadedData = _loadedEventDatas[evt.EventId];
                evt.InitWithEventData(loadedData);
                appliedCount++;
            }
        }
        
        Debug.Log($"イベントデータ適用完了: {appliedCount}個のイベントに適用");
    }

    /// <summary>
    /// 強制的にすべてのイベントデータを再適用する（デバッグ用）
    /// </summary>
    public void ForceApplyAllEventData()
    {
        Debug.Log("強制的にすべてのイベントデータを再適用します。");
        ApplyLoadedDataToSceneEvents();
    }

    /// <summary>
    /// 現在ロードされているイベントデータの情報を表示（デバッグ用）
    /// </summary>
    public void DebugPrintLoadedEventData()
    {
        Debug.Log($"現在ロードされているイベントデータ数: {_loadedEventDatas.Count}");
        foreach (var kvp in _loadedEventDatas)
        {
            Debug.Log($"EventID: {kvp.Key}, EventData: {kvp.Value}");
        }
    }
}
