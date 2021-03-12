using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
    private AudioSource audioSource;
    private bool isPlaying;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayOneShot(AudioClip clip, bool interrupt = true)
    {
        if (clip)
        {
            if (interrupt)
            {
                audioSource.Stop();
            }

            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayOneShot(AudioClip clip, float volume, bool interrupt = true)
    {
        if (clip)
        {
            if (interrupt)
            {
                audioSource.Stop();
            }

            audioSource.PlayOneShot(clip, volume);
        }
    }
}
