using UnityEngine;

public class FallDetector : MonoBehaviour
{
    private GameManager logic;

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2")) // Nếu nhân vật chạm vực
        {
            logic.RespawnPlayer(collision.gameObject); // Quay về checkpoint gần vực nếu còn máu
        }
    }
}
