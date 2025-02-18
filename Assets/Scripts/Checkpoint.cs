using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            GameManager logic = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            logic.SetCheckpoint(transform.position); // Lưu vị trí checkpoint mới
        }
    }
}
