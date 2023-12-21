using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonSound;
    public AudioClip attackSound;
    public AudioClip buffSound;
    public AudioClip debuffSound;
    public AudioClip healSound;

    private Entity entityRef;
    public void OnClickButtonSound()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(buttonSound);
    }

    public void AttackSound()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(attackSound);
        Debug.Log("attackSound");
    }
    public void DebuffSound()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(debuffSound);
        Debug.Log("DebuffSoundSound");
    }
    public void BuffSound()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(buffSound);
        Debug.Log("BuffSound");
    }
    public void HealSound()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(healSound);
        Debug.Log("HealSound");
    }

    public void OnEnable()
    {
        Registry.events.AttackSound += AttackSound;
        Registry.events.BuffSound += BuffSound;
        Registry.events.DebuffSound += DebuffSound;
        Registry.events.HealSound += HealSound;
    }

    public void OnDisable()
    {
        Registry.events.AttackSound -= AttackSound;
        Registry.events.BuffSound -= BuffSound;
        Registry.events.DebuffSound -= DebuffSound;
        Registry.events.HealSound -= HealSound;
    }
}
