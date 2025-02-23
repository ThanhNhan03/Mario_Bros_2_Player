using UnityEngine;

public class CoinScore : MonoBehaviour
{
    [SerializeField] int coinScore = 100;
    private bool isCollected = false; // Biến kiểm tra coin đã bị nhặt hay chưa

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCollected && (other.CompareTag("Player1") || other.CompareTag("Player2")))
        {
            isCollected = true; // Đánh dấu coin đã bị nhặt
            GameManager.instance.AddScore(other.gameObject, coinScore); // Cộng điểm cho đúng người chơi
            Destroy(gameObject); // Xóa coin
        }
    }
}
