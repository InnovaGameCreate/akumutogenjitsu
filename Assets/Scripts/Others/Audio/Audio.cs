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
    /// Audio�̍Đ�(ex: Audio.Instance.Play(0, false);)
    /// </summary>
    /// <param name="index"> Clip��index </param>
    /// <param name="isLoop"> ���[�v���邩 </param>
    public void Play(int index, bool isLoop)
    {
        if (index < 0 || index >= _audioDatas.Length)
        {
            Debug.Log($"�C���f�b�N�X���͈͊O�ł��B({index})");
            return;
        }

        if (_audioDatas[index].Clip == null)
        {
            Debug.Log($"AudioClip���Ȃ��ł��B{index}");
            return;
        }

        if (_audioDatas[index].AudioSource == null)
        {
            Debug.Log("AudioSource���A�^�b�`����Ă��܂���B");
            return;
        }

        _audioDatas[index].AudioSource.loop = isLoop;
        _audioDatas[index].AudioSource.clip = _audioDatas[index].Clip;
        _audioDatas[index].AudioSource.volume = _audioDatas[index].Volume;
        _audioDatas[index].AudioSource.Play();
    }

    /// <summary>
    /// 1�񂾂�SE�𗬂�
    /// </summary>
    /// <param name="index"> Clip��index </param>
    public void PlayOneShot(int index)
    {
        if (index < 0 || index >= _audioDatas.Length)
        {
            Debug.LogError($"�C���f�b�N�X���͈͊O�ł��B({index})");
            return;
        }

        if (_audioDatas[index].Clip == null)
        {
            Debug.LogError($"AudioClip���Ȃ��ł��B{index}");
            return;
        }

        if (_audioDatas[index].AudioSource == null)
        {
            Debug.LogError("AudioSource���A�^�b�`����Ă��܂���B");
            return;
        }

        _audioDatas[index].AudioSource.volume = _audioDatas[index].Volume;
        _audioDatas[index].AudioSource.PlayOneShot(_audioDatas[index].Clip);
    }

    /// <summary>
    /// Audio�̍Đ����~�߂�
    /// </summary>
    public void Stop(int index)
    {
        if (index < 0 || index >= _audioDatas.Length)
        {
            Debug.LogError($"�C���f�b�N�X���͈͊O�ł��B({index})");
            return;
        }

        if (_audioDatas[index].AudioSource == null)
        {
            Debug.LogError("AudioSource���A�^�b�`����Ă��܂���B");
            return;
        }

        _audioDatas[index].AudioSource.Stop();
    }
}
