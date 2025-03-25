using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VictoryPanel : MonoBehaviour
{
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;
    public float displayDuration = 3f;
    public CanvasGroup canvasGroup;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
        
        canvasGroup.alpha = 0;
    }

    public void ShowVictoryPanel()
    {
        // Enable the GameObject first
        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        
        // Find a MonoBehaviour that's active to start the coroutine
        if (AudioManager.instance != null)
        {
            AudioManager.instance.StartCoroutine(VictoryPanelSequence());
        }
        else
        {
            // Fallback to starting the coroutine on this object if it's now active
            StartCoroutine(VictoryPanelSequence());
        }
    }

    private IEnumerator VictoryPanelSequence()
    {
        // Fade in
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

        // Fade out
        elapsedTime = 0;
        while (elapsedTime < fadeOutDuration)
        {
            canvasGroup.alpha = 1 - (elapsedTime / fadeOutDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;
        
        gameObject.SetActive(false);
    }
}