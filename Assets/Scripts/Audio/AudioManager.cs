using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Walk SFX")]
    public AudioData walkSource;

    [Header("Cell SFX")]
    public AudioData[] toiletSource;
    public AudioData sleepSource;

    [Header("Talks")]
    public AudioData[] talkSource;

    [Header("Police SFX")]
    public AudioData[] policeSource;

    [Header("Ambience")]
    public AudioData chowtimeSource;
    public AudioData recTimeSource;

    [Header("Eating SFX")]
    public AudioData[] eatingSource;

    public void PlayAudio(AudioSource source, AudioData data)
    {
        if (source == null || data == null || data.clip == null)
            return;

        source.PlayOneShot(data.clip, data.volume);
    }

    public void PlayAudio(AudioSource source, AudioData[] dataArray)
    {
        if (source == null || dataArray == null || dataArray.Length == 0)
            return;

        int randomIndex = Random.Range(0, dataArray.Length);
        PlayAudio(source, dataArray[randomIndex]);
    }

    public void StopAudio(AudioSource source)
    {
        if (source == null)
            return;

        source.Stop();
    }

    public void PlayAmbience(AudioSource source, AudioData data)
    {
        if (source == null || data == null || data.clip == null)
            return;

        source.clip = data.clip;
        source.volume = data.volume;
        source.loop = true;
        source.Play();
    }
}
