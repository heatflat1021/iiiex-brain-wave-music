using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmotivUnityPlugin;

public class SoundManager : MonoBehaviour
{
    private AudioSource[] sources;

    void Start()
    {
        sources = this.gameObject.GetComponents<AudioSource>();
    }

    public void SoundStart(Channel_t channel, BandPowerType band)
    {
        AudioSource targetAudioSource = SearchAudioSource(channel, band);
        
        if (targetAudioSource == null)
        {
            return;
        }

        if (!targetAudioSource.isPlaying)
        {
            targetAudioSource.Play();
        }
    }

    public void SoundStop(Channel_t channel, BandPowerType band)
    {
        AudioSource targetAudioSource = SearchAudioSource(channel, band);

        if (targetAudioSource == null)
        {
            return;
        }

        if (targetAudioSource.isPlaying)
        {
            targetAudioSource.Stop();
        }
    }

    /// <summary>
    /// Search for the desired AudioSource in sources. If it exists, return the AudioSource, otherwise return null.
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="band"></param>
    /// <returns></returns>
    private AudioSource SearchAudioSource(Channel_t channel, BandPowerType band)
    {
        foreach (AudioSource source in sources)
        {
            if (!source.clip.name.Contains(ChannelStringList.ChannelToString(channel)))
            {
                continue;
            }

            if (!source.clip.name.Contains(BandPowerDataBuffer.BandPowerMap[band]))
            {
                continue;
            }

            return source;
        }

        return null;
    }
}
