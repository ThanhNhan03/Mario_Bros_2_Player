using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreditPanel : MonoBehaviour
{
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;
    public float displayDuration = 5f; 
    public string mainMenuSceneName = "MainMenu"; // Name of your main menu scene
    public CanvasGroup canvasGroup;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
        
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }

    public void ShowCreditPanel()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeInCredits());
    }

    private IEnumerator FadeInCredits()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeInDuration)
        {
            canvasGroup.alpha = elapsedTime / fadeInDuration;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;

        // Wait for display duration
        yield return new WaitForSeconds(displayDuration);

        // Start fade out
        yield return StartCoroutine(FadeOutAndReturnToMenu());
    }

    private IEnumerator FadeOutAndReturnToMenu()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeOutDuration)
        {
            canvasGroup.alpha = 1 - (elapsedTime / fadeOutDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;

        // Load main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuSceneName);
    }
}