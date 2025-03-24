using UnityEngine;
using UnityEngine.SceneManagement;
using System;

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

    public void PlayWarning()  // Add this method
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
        audioSource.Play();
    }

    public void PlayOneShot(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}