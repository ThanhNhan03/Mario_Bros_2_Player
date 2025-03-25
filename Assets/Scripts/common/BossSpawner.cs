using System.Collections;
using UnityEngine;
using TMPro;

public class BossSpawner : MonoBehaviour
{
    public GameObject warningPanel;  
    public TextMeshProUGUI warningText; 
    public float warningDuration = 3f; 
    public float flashSpeed = 0.2f;
    public GameObject boss;
    public float chaseDelay = 1f;

    void Start()
    {
        if (boss != null)
        {
            boss.SetActive(false);
        }
        
        // Use a coroutine to wait for players to be fully initialized
        StartCoroutine(SetupSequence());
    }

    IEnumerator SetupSequence()
    {
        // Wait for a frame to ensure all objects are initialized
        yield return new WaitForEndOfFrame();
        
        // Disable player movement at the start
        DisablePlayerMovement();
        
        // Start the warning sequence
        StartCoroutine(WarningSequence());
    }

    private void DisablePlayerMovement()
    {
        PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();
        foreach (PlayerMovement player in players)
        {
            if (player != null)  // Add null check
            {
                player.SetMovementEnabled(false);
            }
        }
    }

    private void EnablePlayerMovement()
    {
        PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();
        foreach (PlayerMovement player in players)
        {
            if (player != null)  // Add null check
            {
                player.SetMovementEnabled(true);
            }
        }
    }
    
    IEnumerator WarningSequence()
    {
        warningPanel.SetActive(true);
        
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayWarning();
        }

        float elapsedTime = 0f;
        bool isVisible = true;

        while (elapsedTime < warningDuration)
        {
            isVisible = !isVisible;
            warningText.enabled = isVisible;
            yield return new WaitForSeconds(flashSpeed);
            elapsedTime += flashSpeed;
        }

        warningPanel.SetActive(false);

        if (boss != null)
        {
            boss.SetActive(true);
            yield return new WaitForSeconds(chaseDelay);
            
            // Play background music after chase delay
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayBGMForCurrentScene();
            }

            BossChase bossChase = boss.GetComponent<BossChase>();
            if (bossChase != null)
            {
                bossChase.StartChasing();
            }
            
            // Re-enable player movement when boss starts chasing
            EnablePlayerMovement();
        }
    }
}
