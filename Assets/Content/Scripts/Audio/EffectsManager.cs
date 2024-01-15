using UnityEngine;

namespace Audio
{
    public class EffectsManager : MonoBehaviour
    {
        public Events events;
        
        public AudioSource audioSource;
        public AudioClip attackSound;
        public AudioClip buffSound;
        public AudioClip debuffSound;
        public AudioClip healSound;

        private Entity _entityRef;
        
        public void OnEnable()
        {
            events.AttackSound += AttackSound;
            events.BuffSound += BuffSound;
            events.DebuffSound += DebuffSound;
            events.HealSound += HealSound;
        }

        public void OnDisable()
        {
            events.AttackSound -= AttackSound;
            events.BuffSound -= BuffSound;
            events.DebuffSound -= DebuffSound;
            events.HealSound -= HealSound;
        }
        
        private void AttackSound()
        {
            audioSource.loop = false;
            audioSource.PlayOneShot(attackSound);
        }
        
        private void DebuffSound()
        {
            audioSource.loop = false;
            audioSource.PlayOneShot(debuffSound);
        }
        
        private void BuffSound()
        {
            audioSource.loop = false;
            audioSource.PlayOneShot(buffSound);
        }
        
        private void HealSound()
        {
            audioSource.loop = false;
            audioSource.PlayOneShot(healSound);
        }
    }
}