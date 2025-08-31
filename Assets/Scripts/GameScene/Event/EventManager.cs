using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>, ISaveableManager<EventSaveData>
{
    // イベントの全てのデータ
    private Dictionary<string, EventData> _savedEventDatas = new();

    private List<AbstractEvent> _allEventsInScene = new();

    void Update()
    {
        foreach (var eventData in _savedEventDatas.Values)
        {
            UpdateActive(eventData.EventId);
        }
    }

    void OnEnable()
    {
        InitializeAllEventInScene();
    }

    /// <summary>
    /// EventDataを取得する
    /// </summary>
    /// <param name="eventId"> イベントID </param>
    /// <returns> EventData? </returns>
    public EventData GetEventData(string eventId)
    {
        if (!_savedEventDatas.ContainsKey(eventId))
        {
            Debug.LogError("EventDataが存在しないです。");
        }
        return _savedEventDatas[eventId];
    }

    /// <summary>
    /// EventDataの設定
    /// </summary>
    public void SetEventData(string eventId, EventData eventData)
    {
        _savedEventDatas[eventId] = eventData;
    }

    /// <summary>
    /// eEventStatusの設定
    /// </summary>
    public void SetEventStatus(string eventId, eEventStatus status)
    {
        var eventData = _savedEventDatas[eventId];
        eventData.EventStatus = status;
        _savedEventDatas[eventId] = eventData;
    }

    /// <summary>
    /// Enabledの設定
    /// </summary>
    public void SetEventEnabled(string eventId, bool enabled)
    {
        var eventData = _savedEventDatas[eventId];
        eventData.Enabled = enabled;
        _savedEventDatas[eventId] = eventData;
    }

    /// <summary>
    /// Activeかどうかを設定する
    /// </summary>
    /// <param name="eventId"> イベントID </param>
    public void UpdateActive(string eventId)
    {
        SetActive(eventId, IsActive(eventId));
    }

    private void SetActive(string eventId, bool active)
    {
        AbstractEvent ev = GetEventByEventId(eventId);
        ev?.gameObject.SetActive(active);
    }

    private bool IsActive(string eventId)
    {
        EventData data = _savedEventDatas[eventId];

        return IsActiveByStoryLayer(data) && data.EventStatus != eEventStatus.Triggered && IsActiveByEventDataEnable(data);
    }

    // MARK: 全てのイベントを制御

    /// <summary>
    /// シーン内の全てのEventを初期化する
    /// </summary>
    public void InitializeAllEventInScene()
    {
        LoadAllEventInScene();
        SetActiveAllEventInScene();
    }

    /// <summary>
    /// シーン内のイベントを全て保存する
    /// </summary>
    public void SaveAllEventInScene()
    {
        foreach (var ev in _allEventsInScene)
        {
            if (ev == null)
            {
                Debug.LogError("AbstractEventがコンポーネントされていません。");
                continue;
            }
            string eventId = ev.EventId;
            if (_savedEventDatas.ContainsKey(eventId))
            {
                ev.EventData = _savedEventDatas[eventId];
            }
            else
            {
                // 登録していないEventDataがあるときは保存
                _savedEventDatas.Add(eventId, ev.DefaultEventData);
            }
        }
    }

    /// <summary>
    /// StoryLayerが0、またはcurrentStoryLayerと同じときにtrue
    /// </summary>
    /// <param name="eventData"> イベント </param>
    /// <returns> active </returns>
    private bool IsActiveByStoryLayer(EventData eventData)
    {
        int currentStoryLayer = StoryManager.Instance.CurrentStoryLayer;
        AbstractEvent ev = GetEventByEventId(eventData.EventId);
        if (ev == null) return false;

        return ev.StoryLayer == 0 || ev.StoryLayer == currentStoryLayer;
    }

    /// <summary>
    /// TriggerOnceがtrueかつEventStatusがTriggeredのときfalse
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    private bool IsActiveByTriggeredOnce(EventData eventData)
    {
        AbstractEvent ev = GetEventByEventId(eventData.EventId);
        if (ev == null) return false;

        if (ev.TriggerOnce && eventData.EventStatus == eEventStatus.Triggered)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Enabled
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    private bool IsActiveByEventDataEnable(EventData eventData)
    {
        return eventData.Enabled;
    }
    
    /// <summary>
    /// 全てのイベントの初期化をする
    /// </summary>
    private void SetActiveAllEventInScene()
    {
        foreach (var eventData in _savedEventDatas)
        {
            AbstractEvent ev = GetEventByEventId(eventData.Value.EventId);
            if (ev == null)
            {
                // 別シーンのイベントのとき
                continue;
            }

            if (!IsActiveByTriggeredOnce(eventData.Value) && !IsActiveByStoryLayer(eventData.Value) && !IsActiveByEventDataEnable(eventData.Value))
            {
                ev.gameObject.SetActive(false);
            }
            else
            {
                ev.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// シーン内のイベントを全て読み込む
    /// </summary>
    private void LoadAllEventInScene()
    {
        _allEventsInScene.Clear();
        // シーン内のEventを取得
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Event");
        foreach (var obj in objs)
        {
            AbstractEvent ev = obj.GetComponent<AbstractEvent>();
            if (ev == null)
            {
                Debug.LogError("AbstractEventがコンポーネントされていません。");
                continue;
            }
            _allEventsInScene.Add(ev);
            string eventId = ev.EventId;
            if (!_savedEventDatas.ContainsKey(eventId))
            {
                _savedEventDatas.Add(eventId, ev.DefaultEventData);
            }
        }
    }

    private AbstractEvent GetEventByEventId(string eventId)
    {
        foreach (var ev in _allEventsInScene)
        {
            if (ev == null)
            {
                Debug.LogError("AbstractEventをコンポーネントしていません。");
                continue;
            }

            if (ev.EventId == eventId)
            {
                return ev;
            }
        }

        return null;
    }

    // MARK: セーブ機能

    public EventSaveData EncodeToSaveData()
    {
        EventSaveData saveData = new EventSaveData();
        saveData.CurrentStoryLayer = StoryManager.Instance.CurrentStoryLayer;
        saveData.EventData = _savedEventDatas;

        return saveData;
    }

    public void LoadFromSaveData(EventSaveData saveData)
    {
        StoryManager.Instance.CurrentStoryLayer = saveData.CurrentStoryLayer;
        _savedEventDatas = saveData.EventData;
	}
}
