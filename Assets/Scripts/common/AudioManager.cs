using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Serializable]
    public class SceneBGM
    {
        public string sceneName;
        public AudioClip bgmClip;
    }

    public SceneBGM[] sceneBGMs;
    public AudioClip gameOverClip;
    public AudioClip levelExitClip;
    public AudioClip warningClip;  
    public AudioClip bossExplosionSound;
    public AudioClip victorySound;
    public AudioClip creditMusic;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Initialize AudioSource if it doesn't exist
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                Debug.Log("Added AudioSource component");
            }
            
            // Set audio source to loop by default
            audioSource.loop = true;
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayBGMForCurrentScene();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMForCurrentScene();
    }

    public void PlayBGMForCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        // Find the matching BGM for the current scene
        foreach (SceneBGM sceneBGM in sceneBGMs)
        {
            if (sceneBGM.sceneName == currentSceneName && sceneBGM.bgmClip != null)
            {
                PlayAudioClip(sceneBGM.bgmClip);
                return;
            }
        }
    }

    public void PlayGameOver()
    {
        PlayAudioClip(gameOverClip);
    }

    public void PlayLevelExit()
    {
        PlayAudioClip(levelExitClip);
    }

    public void PlayWarning()  
    {
        PlayAudioClip(warningClip);
    }

    public void ResumeBGM()
    {
        PlayBGMForCurrentScene();
    }

    private void PlayAudioClip(AudioClip clip)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.clip = clip;
        
        // Enable looping for background music, disable for one-time sounds
        if (clip == gameOverClip || clip == levelExitClip || clip == warningClip || 
            clip == victorySound || clip == creditMusic)  // Added creditMusic here
        {
            audioSource.loop = false;
        }
        else
        {
            audioSource.loop = true;
        }
        
        audioSource.Play();
    }

    public void PlayBossExplosion()
    {
        // Stop current music before playing explosion
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        PlayOneShot(bossExplosionSound);
    }

    public void PlayVictory()
    {
        // Wait until explosion sound is finished
        StartCoroutine(PlayVictoryAfterExplosion());
    }

    private IEnumerator PlayVictoryAfterExplosion()
    {
        if (bossExplosionSound != null)
        {
            yield return new WaitForSeconds(bossExplosionSound.length);
        }
        PlayAudioClip(victorySound);
        
        // Wait for victory music to finish
        if (victorySound != null)
        {
            yield return new WaitForSeconds(victorySound.length);
        }
        
        // Play credit music
        PlayAudioClip(creditMusic);
    }

    public void PlayOneShot(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}