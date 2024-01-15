using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Audio
{
    public class MusicManager : MonoBehaviour
    {
        [Header("REFERENCES")]
        public AudioSource audioSource;
        public Events events;
        
        [Header("SOUNGS")]
        public AudioClip introSong;
        public AudioClip loopSong;
        public AudioClip introBattleSong;
        public AudioClip loopBattleSong;
        public AudioClip graveyardSong;
        public AudioClip restSong;

        
        private string _currentScene;
        private AudioClip _audioClip;
        private bool _stopAsync;

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

            _currentScene = SceneManager.GetActiveScene().name;
            
            switch (_currentScene)
            {
                case "mainmenu":
                    PlayintroSong();
                    LaunchLoop(introSong.length);
                    break;
                case "GameBattle":
                    PlayIntroBattleSong();
                    LaunchLoop(introBattleSong.length);
                    break;
                case "GameGraveyard":
                    PlayGraveyardSong();
                    LaunchLoop(graveyardSong.length);
                    break;
                case "GameBonfire":
                    PlayRestSong();
                    LaunchLoop(restSong.length);
                    break;
            }
        }

        private void PlayintroSong()
        {
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.PlayOneShot(introSong);
        }

        private void PlayloopSong()
        {
            audioSource.Stop();
            audioSource.loop = true;
            audioSource.clip = loopSong;
            audioSource.Play();  
        }

        private void PlayIntroBattleSong()
        {
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.PlayOneShot(introBattleSong);
        }

        private void PlayLoopBattleSong()
        {
            audioSource.Stop();
            audioSource.loop = true;
            audioSource.clip = loopBattleSong;
            audioSource.Play();
        }

        private void PlayGraveyardSong()
        {
            audioSource.Stop();
            audioSource.loop = true;
            audioSource.clip = graveyardSong;
            audioSource.Play();
        }
        private void PlayRestSong()
        {
            audioSource.Stop();
            audioSource.loop = true;
            audioSource.clip = restSong;
            audioSource.Play();
        }

        private async void LaunchLoop(float waitingDuration)
        {
            await Task.Delay(Mathf.RoundToInt(waitingDuration * 1000));
            _currentScene = SceneManager.GetActiveScene().name;

            if (_stopAsync || !this) return;
            
            switch (_currentScene)
            {
                case "mainmenu":
                    PlayloopSong();
                    break;
                case "GameBattle":
                    PlayLoopBattleSong();
                    break;
                case "GameGraveyard":
                    PlayGraveyardSong();
                    break;
                case "GameBonfire":
                    PlayRestSong();
                    break;
            }
        }

        private void OnRestStopPlaying() => audioSource.Stop();
        private void OnEnable() => events.onRestClick += OnRestStopPlaying;
        private void OnDisable()
        {
            _stopAsync = true;
            events.onRestClick -= OnRestStopPlaying;
        }
    }
}
