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

    private void Awake()
    {
        if (CheckInstance())
        {
            DontDestroyOnLoad(this);
        }
    }

    /// <summary>
    /// Audioの再生(ex: Audio.Instance.Play(0, false);)
    /// </summary>
    /// <param name="index"> Clipのindex </param>
    /// <param name="isLoop"> ループするか </param>
    public void Play(int index, bool isLoop)
    {
        if (index < 0 || index >= _audioDatas.Length)
        {
            Debug.Log($"インデックスが範囲外です。({index})");
            return;
        }

        if (_audioDatas[index].Clip == null)
        {
            Debug.Log($"AudioClipがないです。{index}");
            return;
        }

        if (_audioDatas[index].AudioSource == null)
        {
            Debug.Log("AudioSourceがアタッチされていません。");
            return;
        }

        _audioDatas[index].AudioSource.loop = isLoop;
        _audioDatas[index].AudioSource.clip = _audioDatas[index].Clip;
        _audioDatas[index].AudioSource.volume = _audioDatas[index].Volume;
        _audioDatas[index].AudioSource.Play();
    }

    /// <summary>
    /// 1回だけSEを流す
    /// </summary>
    /// <param name="index"> Clipのindex </param>
    public void PlayOneShot(int index)
    {
        if (index < 0 || index >= _audioDatas.Length)
        {
            Debug.LogError($"インデックスが範囲外です。({index})");
            return;
        }

        if (_audioDatas[index].Clip == null)
        {
            Debug.LogError($"AudioClipがないです。{index}");
            return;
        }

        if (_audioDatas[index].AudioSource == null)
        {
            Debug.LogError("AudioSourceがアタッチされていません。");
            return;
        }

        _audioDatas[index].AudioSource.volume = _audioDatas[index].Volume;
        _audioDatas[index].AudioSource.PlayOneShot(_audioDatas[index].Clip);
    }

    /// <summary>
    /// Audioの再生を止める
    /// </summary>
    public void Stop(int index)
    {
        if (index < 0 || index >= _audioDatas.Length)
        {
            Debug.LogError($"インデックスが範囲外です。({index})");
            return;
        }

        if (_audioDatas[index].AudioSource == null)
        {
            Debug.LogError("AudioSourceがアタッチされていません。");
            return;
        }

        _audioDatas[index].AudioSource.Stop();
    }
}
