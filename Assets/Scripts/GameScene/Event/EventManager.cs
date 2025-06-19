using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
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
        return _loadedEventDatas[eventId];
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
}
