using UnityEngine;

public class BossChase : MonoBehaviour
{
    public Transform player;
    public float startSpeed = 5f; 
    public float acceleration = 0.2f; 
    private float currentSpeed;

    void Start()
    {
        currentSpeed = startSpeed;
    }

    void Update()
    {
        if (player == null) return;

        // Boss di chuyển về phía người chơi theo trục X
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.position.x, transform.position.y), currentSpeed * Time.deltaTime);

        // Tăng tốc độ theo thời gian
        currentSpeed += acceleration * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            // Khi boss bắt được người chơi
            Debug.Log("Bạn đã bị bắt!");
            // Thực hiện hành động như Restart game hoặc Game Over
        }
    }
}
