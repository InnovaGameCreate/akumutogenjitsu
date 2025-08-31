using UnityEngine;

public class StoryManager : Singleton<StoryManager>
{
    [SerializeField] private int _storyLayer = 1;
    
    /// <summary>
    /// 現在のStoryLayer(1以上)
    /// </summary>
    public int CurrentStoryLayer
    {
        get
        {
            return _storyLayer;
        }
        set
        {
            if (value < 1)
            {
                Debug.Log($"StoryLayerは1以上です。({value})");
                return;
            }
            _storyLayer = value;
        }
    }
}
