using UnityEngine;

public struct EventData
{
    private eEvent _eventType;
    private string _eventId;
    private int _storyLayer;
    private eEventStatus _eventStatus;
    private bool _enabled;

    public eEvent EventType
    {
        get
        {
            return _eventType;
        }
        set
        {
            _eventType = value;
        }
    }

    /// <summary>
    /// イベントID
    /// </summary>
    public string EventId
    {
        get
        {
            return _eventId;
        }
        set
        {
            if (_eventId != value)
            {
                if (_eventId != null)
                {
                    Debug.Log($"EventIDが変更されました。([{_eventId}] -> [{value}])");
                }
                _eventId = value;
            }
        }
    }

    /// <summary>
    /// StoryLayer(0のときは常に有効で、指定するときは1以上に設定する。)
    /// </summary>
    public int StoryLayer
    {
        get
        {
            return _storyLayer;
        }
        set
        {
            if (value < 0)
            {
                Debug.LogError($"EventLayerは0以上を設定してください。({value})");
                return;
            }
            _storyLayer = value;
        }
    }

    /// <summary>
    /// Eventの状態
    /// </summary>
    public eEventStatus EventStatus
    {
        get
        {
            return _eventStatus;
        }
        set
        {
            _eventStatus = value;
        }
    }

    /// <summary>
    /// Eventが有効か
    /// </summary>
    public bool Enabled
    {
        get
        {
            return _enabled;
        }
        set
        {
            _enabled = value;
        }
    }
}