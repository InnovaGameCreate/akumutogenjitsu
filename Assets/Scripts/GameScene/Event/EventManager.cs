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
                Debug.LogError($"イベントがコンポーネントされていません。(Event: {obj})");
            }
            else
            {
                SaveEventData(evt.EventId, evt.EventData);
            }
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
    /// シーン上のイベントのEventDataをロードする
    /// </summary>
    public void LoadAllEventInScene()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Event"))
        {
            AbstractEvent evt = obj.GetComponent<AbstractEvent>();
            if (evt != null && HasLoadedEvent(evt.EventId))
            {
                evt.InitWithEventData(_loadedEventDatas[evt.EventId]);
            }
        }
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
    }
}
