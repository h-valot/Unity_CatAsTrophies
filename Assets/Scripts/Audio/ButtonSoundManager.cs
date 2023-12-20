using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonSound;
    public void OnClickButtonSound()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(buttonSound);
    }
}
