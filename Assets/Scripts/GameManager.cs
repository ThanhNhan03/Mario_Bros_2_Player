using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int player1Score = 0;
    public int player2Score = 0;

    public Text player1ScoreText;
    public Text player2ScoreText;

    // Add references for player health UI
    public Text player1HealthText;
    public Text player2HealthText;

    private GameObject player1;
    private GameObject player2;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject); // Giữ lại GameManager khi chuyển Scene
        }
        //else
        //{
        //    Destroy(gameObject); // Nếu đã có GameManager khác, hủy bỏ
        //}
    }

    void Update()
    {
        player1 = GameObject.FindWithTag("Player1");
        player2 = GameObject.FindWithTag("Player2");

        // Cập nhật UI Player1
        player1ScoreText.text = "Player 1: " + player1Score;
        if (player1 != null)
        {
            PlayerHealth player1Health = player1.GetComponent<PlayerHealth>();
            player1HealthText.text = "HP: " + player1Health.Health;
            player1HealthText.gameObject.SetActive(true);
        }
        else
        {
            player1HealthText.gameObject.SetActive(false);
        }

        if (player2 != null)
        {
            player2ScoreText.text = "Player 2: " + player2Score;
            player2ScoreText.gameObject.SetActive(true);

            PlayerHealth player2Health = player2.GetComponent<PlayerHealth>();
            player2HealthText.text = "HP: " + player2Health.Health;
            player2HealthText.gameObject.SetActive(true);
        }
        else
        {
            player2ScoreText.gameObject.SetActive(false);
            player2HealthText.gameObject.SetActive(false);
        }
    }

    public void AddScore(GameObject player, int points)
    {
        if (player.CompareTag("Player1"))
        {
            player1Score += points;
            player1ScoreText.text = "Player 1: " + player1Score;
        }
        else if (player.CompareTag("Player2"))
        {
            player2Score += points;
            player2ScoreText.text = "Player 2: " + player2Score;
        }
    }
}
