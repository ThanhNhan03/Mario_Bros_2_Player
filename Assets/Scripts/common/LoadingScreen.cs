using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public Slider progressBar; 
    public float fakeLoadTime = 3f; 

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

       
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }
}
