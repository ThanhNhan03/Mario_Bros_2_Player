using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int playerScore;
    public int playerHealth = 3; // Máu ban đầu của nhân vật
    public Text textScore;
    public Text textHealth;

    private Vector3 lastCheckpoint;   // Checkpoint toàn game
    private Vector3 lastSafePosition; // Vị trí an toàn gần vực

    void Start()
    {
        GameObject checkpoint = GameObject.FindGameObjectWithTag("Respawn");
        if (checkpoint != null)
        {
            lastCheckpoint = checkpoint.transform.position;
            lastSafePosition = lastCheckpoint; // Ban đầu đặt giống checkpoint
        }

        //UpdateUI();
    }

    public void addScore(int score)
    {
        playerScore += score;
        //UpdateUI();
    }

    public void reduceHealth(int damage)
    {
        playerHealth -= damage;
        //UpdateUI();

        if (playerHealth <= 0)
        {
            Debug.Log("Game Over! Restarting...");
            RestartGame();
        }
    }

    public void RespawnPlayer(GameObject player)
    {
        reduceHealth(1); // Trừ 1 máu khi rơi xuống vực

        if (playerHealth > 0)
        {
            player.transform.position = lastSafePosition; // Quay về vị trí gần vực nếu còn máu
        }
    }

    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        lastCheckpoint = newCheckpoint;
        lastSafePosition = newCheckpoint; // Cập nhật vị trí an toàn gần vực
    }

    public void UpdateSafePosition(Vector3 newSafePosition)
    {
        lastSafePosition = newSafePosition; // Cập nhật vị trí an toàn gần vực
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //void UpdateUI()
    //{
    //    textScore.text = "Score: " + playerScore.ToString();
    //    textHealth.text = "HP: " + playerHealth.ToString();
    //}
}
