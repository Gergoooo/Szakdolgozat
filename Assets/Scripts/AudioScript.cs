using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] public AudioSource SFXSource;
    [Header("Clips")]

    public AudioClip planeEngine;
    public AudioClip Shooting;
    public AudioClip Explosion;

    private float minVolume = 0.1f;
    private float maxVolume = 1f;
    private float minPitch = 0.5f;
    private float maxPitch = 2f;

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayLoopingEngineSound()
    {
        SFXSource.clip = planeEngine;
        SFXSource.loop = true;
        SFXSource.Play();
    }

    public void SetVolume(float volume)
    {
        SFXSource.volume = Mathf.Clamp(SFXSource.volume + volume, minVolume, maxVolume);
    }

    public void SetPitch(float pitch)
    {
        SFXSource.pitch = Mathf.Clamp(SFXSource.pitch + pitch, minPitch, maxPitch);
    }

    public void StopEngineSound()
    {
        if (SFXSource != null && SFXSource.clip == planeEngine)
        {
            SFXSource.Stop();
        }
    }
}
