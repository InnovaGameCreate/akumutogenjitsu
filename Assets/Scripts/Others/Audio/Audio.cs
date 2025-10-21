using UnityEngine;

public class Audio : Singleton<Audio>
{
    [System.Serializable]
    public struct AudioData
    {
        [SerializeField] public AudioClip clip;
        [SerializeField, Range(0f, 1f)]
        public float volume;
    }

    /// <summary>
    /// BGMを流すAudioSource
    /// </summary>
    private AudioSource _bgmAudioSource;

    /// <summary>
    /// SEを流すAudioSource
    /// </summary>
    private AudioSource _seAudioSource;

    /// <summary>
    /// Audio の操作 (例: Audio.Instance.Play(0, false);)
    /// </summary>
    /// <param name="index">Clip の index</param>
    /// <param name="isLoop">ループ設定</param>
    public void PlayBgm(AudioData audioData, bool isLoop)
    {
        if (audioData.clip == null)
        {
            Debug.Log($"AudioClip がありません");
            return;
        }

        if (_bgmAudioSource == null)
        {
            Debug.Log("AudioSource がアタッチされていません");
            return;
        }

        _bgmAudioSource.loop = isLoop;
        _bgmAudioSource.clip = _bgmAudioSource.clip;
        _bgmAudioSource.volume = _bgmAudioSource.volume;
        _bgmAudioSource.Play();
    }

    /// <summary>
    /// 1回だけ再生する
    /// </summary>
    /// <param name="index">Clip の index</param>
    public void PlaySe(AudioData audioData)
    {
        if (audioData.clip == null)
        {
            Debug.LogError($"AudioClip がありません");
            return;
        }

        if (_seAudioSource == null)
        {
            Debug.LogError("AudioSource がアタッチされていません");
            return;
        }

        _seAudioSource.volume = audioData.volume;
        _seAudioSource.PlayOneShot(audioData.clip);
    }

    /// <summary>
    /// Audio の停止
    /// </summary>
    public void StopBgm()
    {
        if (_bgmAudioSource == null)
        {
            Debug.LogError("AudioSource がアタッチされていません");
            return;
        }

        _bgmAudioSource.Stop();
    }
}
