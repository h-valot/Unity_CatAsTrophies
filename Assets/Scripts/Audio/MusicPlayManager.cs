using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class MusicPlayManager : MonoBehaviour
{
    public AudioClip introSong;
    public AudioClip loopSong;
    public AudioClip introBattleSong;
    public AudioClip loopBattleSong;
    public AudioClip graveyardSong;
    public AudioClip restSong;

    public AudioSource audioSource;
    private string currentScene;
    private AudioClip audioClip;
    private bool stopAsync = false;

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

        currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "mainmenu") 
        {
            PlayintroSong();
            LaunchLoop(introSong.length);
            Debug.Log(introSong.length);
        }

        else if (currentScene == "GameBattle")
        {
            PlayIntroBattleSong();
            LaunchLoop(introBattleSong.length);
            Debug.Log(introBattleSong.length);
        }

        else if (currentScene == "GameGraveyard")
        {
            PlayGraveyardSong();
            LaunchLoop(graveyardSong.length);
            Debug.Log(graveyardSong.length);
        }

        else if (currentScene == "GameBonfire")
        {
            PlayRestSong();
            LaunchLoop(restSong.length);
            Debug.Log(restSong.length);
        }
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
        audioSource.clip = loopSong;
        audioSource.Play();  
    }

    public void PlayIntroBattleSong()
    {
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.PlayOneShot(introBattleSong);
    }

    public void PlayLoopBattleSong()
    {
        audioSource.Stop();
        audioSource.loop = true;
        audioSource.clip = loopBattleSong;
        audioSource.Play();
    }

    public void PlayGraveyardSong()
    {
        audioSource.Stop();
        audioSource.loop = true;
        audioSource.clip = graveyardSong;
        audioSource.Play();
    }
    public void PlayRestSong()
    {
        audioSource.Stop();
        audioSource.loop = true;
        audioSource.clip = restSong;
        audioSource.Play();
    }

    private async void LaunchLoop(float _timerToWait)
    {
        await Task.Delay((int)(_timerToWait * 1000));
        currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("Le timer est fini");

        if (!stopAsync && this)
        {
            if (currentScene == "mainmenu")
            {
                PlayloopSong();
                Debug.Log("Tu devrais entendre la loop");
            }

            else if (currentScene == "GameBattle")
            {
                PlayLoopBattleSong();
                Debug.Log("Tu devrais entendre la Battleloop");
            }

            else if (currentScene == "GameGraveyard")
            {
                PlayGraveyardSong();
            }

            else if (currentScene == "GameBonfire")
            {
                PlayRestSong();
            }
        }
    }

    private void OnDisable()
    {
        stopAsync = true;
    }
}
