using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayManager : MonoBehaviour
{
    public AudioClip introSong;
    public AudioClip loopSong;

    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        if ((introSong == null) || (loopSong == null))
        {
            string error = "";
            if (introSong == null)
                error = "The Regular Audio Clip has not been initialized in the inspector, please do this now. ";
            if (loopSong == null)
                error += "The High Damage Audio Clip has not been initialized in the inspector, please do this now. ";
            Debug.LogError(error);
        }

        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource component missing from this gameobject. Adding one.");
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        PlayintroSong();
    }

    public void PlayintroSong()
    {
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.PlayOneShot(introSong);
    }

    public void PlayloopSong()
    {
        audioSource.Stop();
        audioSource.loop = true;
        audioSource.PlayOneShot(loopSong);
    }
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayloopSong();
        }
    }
}
