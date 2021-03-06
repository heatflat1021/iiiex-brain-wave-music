using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmotivUnityPlugin;

public class SoundManager : MonoBehaviour
{
    private AudioSource[] sources;

    private const float VolumeFadeOutAmount = 0.2f;

    void Start()
    {
        sources = this.gameObject.GetComponents<AudioSource>();
    }

    public void SoundStart(CerebrumArea.CerebrumArea_t cerebrumArea, BandPowerType band)
    {
        AudioSource targetAudioSource = SearchAudioSource(cerebrumArea, band);
        
        if (targetAudioSource == null)
        {
            return;
        }

        if (targetAudioSource.isPlaying)
        {
            return;
        }

        targetAudioSource.volume = 1;
        targetAudioSource.Play();
    }

    public void SoundStop(CerebrumArea.CerebrumArea_t cerebrumArea, BandPowerType band)
    {
        AudioSource targetAudioSource = SearchAudioSource(cerebrumArea, band);

        if (targetAudioSource == null)
        {
            return;
        }

        if (!targetAudioSource.isPlaying)
        {
            return;
        }

        if (VolumeFadeOutAmount < targetAudioSource.volume)
        {
            targetAudioSource.volume -= VolumeFadeOutAmount;
        }
        else
        {
            targetAudioSource.Stop();
        }
    }

    /// <summary>
    /// Search for the desired AudioSource in sources. If it exists, return the AudioSource, otherwise return null.
    /// </summary>
    /// <param name="cerebrumArea"></param>
    /// <param name="band"></param>
    /// <returns></returns>
    private AudioSource SearchAudioSource(CerebrumArea.CerebrumArea_t cerebrumArea, BandPowerType band)
    {
        foreach (AudioSource source in sources)
        {
            if (!source.clip.name.Contains(CerebrumArea.ConvertCerebrumAreaTToString(cerebrumArea)))
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
