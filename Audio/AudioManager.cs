using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class AudioManager : PersistanSingleton<AudioManager>
{
    [SerializeField] AudioSource sFXPlayer;
    const float MAX_PITCH = 1.1f;
    const float MIN_PITCH = 0.9f;
    public void PlaySFX(AudioData audioData)//适合播放UI音效
    {
        //sFXPlayer.clip = audioClip;
        //sFXPlayer.volume = volume;
        //sFXPlayer.Play();//Play函数一次只能播放一个音频片段，无法多个播放
        sFXPlayer.PlayOneShot(audioData.audioClip, audioData.volume);
    }
    public void PlayRandomSFX(AudioData audioData)//随机调整音高营造不同的感觉,适合播放连续的音效片段
    {
        sFXPlayer.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
        PlaySFX(audioData);
    }
    public void PlayRandomSFX(AudioData[] audioData)//随机播放音效
    {
        PlayRandomSFX(audioData[Random.Range(0, audioData.Length)]);
    }
}
[System.Serializable]
public class AudioData
{
    public AudioClip audioClip;
    public float volume = 1f;
}