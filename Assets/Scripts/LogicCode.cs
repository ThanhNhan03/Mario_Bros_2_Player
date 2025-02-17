using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicCode : MonoBehaviour
{
    public int playerScore;
    public int playerHealth = 3; // Máu ban đầu của nhân vật
    public Text textScore;
    public Text textHealth; // Thêm UI hiển thị máu

    public void addScore(int score)
    {
        playerScore += score;
        textScore.text = " " + playerScore.ToString();
    }

    public void reduceHealth(int damage)
    {
        playerHealth -= damage;
        textHealth.text = " "+ playerHealth.ToString();

        if (playerHealth <= 0)
        {
            Debug.Log("Game Over! Restarting...");
            RestartGame(); // Gọi hàm restart game
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Load lại scene hiện tại
    }
}
