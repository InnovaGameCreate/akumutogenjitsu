using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // ロードされたイベントのイベントID
    private Dictionary<string, eEventStatus> _loadedEventStatuses = new Dictionary<string, eEventStatus>();

    /// <summary>
    /// シーン上のイベントのEventStatusをセーブする
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
            else if (!_loadedEventStatuses.ContainsKey(evt.EventId))
            {
                _loadedEventStatuses.Add(evt.EventId, evt.EventStatus);
            }
        }
    }

    /// <summary>
    /// イベントがロードされているか
    /// </summary>
    /// <param name="eventId"> イベントID </param>
    /// <returns> 読み込まれているか </returns>
    public bool HasLoadedEvent(string eventId)
    {
        return _loadedEventStatuses.ContainsKey(eventId);
    }

    /// <summary>
    /// シーン上のイベントのEventStatusをロードする
    /// </summary>
    public void LoadAllEventInScene()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Event"))
        {
            AbstractEvent evt = obj.GetComponent<AbstractEvent>();
            if (HasLoadedEvent(evt.EventId))
            {
                evt.EventStatus = _loadedEventStatuses[evt.EventId];
            }
        }
    }
}
