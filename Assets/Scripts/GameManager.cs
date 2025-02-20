using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int player1Score = 0;
    public int player2Score = 0;

    public Text player1ScoreText;
    public Text player2ScoreText;

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
        // Cập nhật Player2 mỗi frame để kiểm tra sự tồn tại
        player2 = GameObject.FindWithTag("Player2");

        // Cập nhật UI Player1
        player1ScoreText.text = "Player 1: " + player1Score;

        // Nếu Player2 tồn tại, hiển thị điểm số
        if (player2 != null)
        {
            player2ScoreText.text = "Player 2: " + player2Score;
            player2ScoreText.gameObject.SetActive(true);
        }
        else
        {
            player2ScoreText.gameObject.SetActive(false);
        }
    }

    // Hàm cộng điểm chính xác cho Player1 hoặc Player2
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
