using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private bool player1In = false;
    private bool player2In = false;
    private GameObject player1;
    private GameObject player2;

    private void Update()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1"))
        {
            player1In = true;
            Debug.Log("Player 1 entered the exit area.");
            collision.GetComponent<PlayerMovement>().SetMovementEnabled(false); // Disable movement for Player 1
        }
        if (collision.CompareTag("Player2"))
        {
            player2In = true;
            Debug.Log("Player 2 entered the exit area.");
            collision.GetComponent<PlayerMovement>().SetMovementEnabled(false); // Disable movement for Player 2
        }
    
        bool isPlayer2Present = player2 != null;
    
        if (player1In && (!isPlayer2Present || player2In))
        {
            Debug.Log("Players are in the exit area. Loading next level...");
            
            Player2Spawn.shouldSpawnPlayer2 = isPlayer2Present;
            
            LoadNextLevel();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1"))
        {
            player1In = false;
            Debug.Log("Player 1 left the exit area.");
        }
        if (collision.CompareTag("Player2"))
        {
            player2In = false;
            Debug.Log("Player 2 left the exit area.");
        }
    }

    void LoadNextLevel()
    {
        AudioManager.instance.PlayLevelExit();
        StartCoroutine(WaitForLevelExitAudio());
    }

    IEnumerator WaitForLevelExitAudio()
    {
        yield return new WaitForSeconds(AudioManager.instance.levelExitClip.length);
    
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        string nextSceneName = SceneUtility.GetScenePathByBuildIndex(currentSceneIndex + 1);
        nextSceneName = System.IO.Path.GetFileNameWithoutExtension(nextSceneName);
        Debug.Log("Loading next level: " + nextSceneName);
        
        LoadingScreen.LoadScene(nextSceneName);
        
        // Play the BGM for the new scene
        AudioManager.instance.PlayBGMForCurrentScene();
    }
}
