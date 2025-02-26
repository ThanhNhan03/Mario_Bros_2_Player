using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private bool player1In = false;
    private bool player2In = false;
    private GameObject player2;

    private void Update()
    {
        player2 = GameObject.FindGameObjectWithTag("Player2");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1"))
        {
            player1In = true;
            Debug.Log("Player 1 entered the exit area.");
        }
        if (collision.CompareTag("Player2"))
        {
            player2In = true;
            Debug.Log("Player 2 entered the exit area.");
        }

        if (player1In && (player2 == null || player2In))
        {
            Debug.Log("Both players are in the exit area. Loading next level...");
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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Loading next level: " + (currentSceneIndex + 1));
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
