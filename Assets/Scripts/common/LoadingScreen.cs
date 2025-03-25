using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public Slider progressBar; 
    public float fakeLoadTime = 3f;
    
    private static string sceneToLoad;
    
    public static void LoadScene(string sceneName)
    {
        sceneToLoad = sceneName;
        SceneManager.LoadScene("LoadingScreen");
    }

    void Start()
    {
        StartCoroutine(FakeLoad());
    }

    IEnumerator FakeLoad()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fakeLoadTime)
        {
            elapsedTime += Time.deltaTime;
            progressBar.value = elapsedTime / fakeLoadTime;
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad ?? "Level1");
    }
}
