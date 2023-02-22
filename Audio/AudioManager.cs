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
    public void PlaySFX(AudioData audioData)//�ʺϲ���UI��Ч
    {
        //sFXPlayer.clip = audioClip;
        //sFXPlayer.volume = volume;
        //sFXPlayer.Play();//Play����һ��ֻ�ܲ���һ����ƵƬ�Σ��޷��������
        sFXPlayer.PlayOneShot(audioData.audioClip, audioData.volume);
    }
    public void PlayRandomSFX(AudioData audioData)//�����������Ӫ�첻ͬ�ĸо�,�ʺϲ�����������ЧƬ��
    {
        sFXPlayer.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
        PlaySFX(audioData);
    }
    public void PlayRandomSFX(AudioData[] audioData)//���������Ч
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