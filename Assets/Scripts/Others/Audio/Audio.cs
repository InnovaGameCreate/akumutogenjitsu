using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Audio : Singleton<Audio>
{
    [System.Serializable]
    public struct AudioData
    {
        [SerializeField] public AudioSource AudioSource;
        [SerializeField] public AudioClip Clip;
        [SerializeField, Range(0f, 1f)]
        public float Volume;
    }

    // Audio Audio Clip
    [SerializeField] private AudioData[] _audioDatas;

    /// <summary>
    /// Audio の操作 (例: Audio.Instance.Play(0, false);)
    /// </summary>
    /// <param name="index">Clip の index</param>
    /// <param name="isLoop">ループ設定</param>
    public void Play(int index, bool isLoop)
    {
        if (index < 0 || index >= _audioDatas.Length)
        {
            Debug.Log($"AudioClip の index が不正です({index})");
            return;
        }

        if (_audioDatas[index].Clip == null)
        {
            Debug.Log($"AudioClip がありません {index}");
            return;
        }

        if (_audioDatas[index].AudioSource == null)
        {
            Debug.Log("AudioSource がアタッチされていません");
            return;
        }

        _audioDatas[index].AudioSource.loop = isLoop;
        _audioDatas[index].AudioSource.clip = _audioDatas[index].Clip;
        _audioDatas[index].AudioSource.volume = _audioDatas[index].Volume;
        _audioDatas[index].AudioSource.Play();
    }

    /// <summary>
    /// 1回だけ再生する
    /// </summary>
    /// <param name="index">Clip の index</param>
    public void PlayOneShot(int index)
    {
        if (index < 0 || index >= _audioDatas.Length)
        {
            Debug.LogError($"AudioClip の index が不正です({index})");
            return;
        }

        if (_audioDatas[index].Clip == null)
        {
            Debug.LogError($"AudioClip がありません {index}");
            return;
        }

        if (_audioDatas[index].AudioSource == null)
        {
            Debug.LogError("AudioSource がアタッチされていません");
            return;
        }

        _audioDatas[index].AudioSource.volume = _audioDatas[index].Volume;
        _audioDatas[index].AudioSource.PlayOneShot(_audioDatas[index].Clip);
    }

    /// <summary>
    /// Audio の停止
    /// </summary>
    public void Stop(int index)
    {
        if (index < 0 || index >= _audioDatas.Length)
        {
            Debug.LogError($"AudioClip の index が不正です({index})");
            return;
        }

        if (_audioDatas[index].AudioSource == null)
        {
            Debug.LogError("AudioSource がアタッチされていません");
            return;
        }

        _audioDatas[index].AudioSource.Stop();
    }
}
