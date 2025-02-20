using UnityEngine;

public class CoinScore : MonoBehaviour
{
    public GameManager logic;

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2")) // Kiểm tra nếu nhân vật chạm vào
        {
            logic.addScore(1);  // Tăng điểm số
            Destroy(gameObject); // Xóa vật thể (coin)
        }
    }
}
