using UnityEngine;

namespace Audio
{
    public class EffectsManager : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip attackSound;
        public AudioClip buffSound;
        public AudioClip debuffSound;
        public AudioClip healSound;

        private Entity _entityRef;
        
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