using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public LogicCode logic;

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LogicCode>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            // Lấy vị trí va chạm
            Vector3 playerPosition = collision.transform.position;
            Vector3 enemyPosition = transform.position;

            // Nếu nhân vật chạm vào từ phía trên (jump kill) thì không trừ máu
            if (playerPosition.y > enemyPosition.y + 0.2f) // Thêm độ lệch 0.2 để tránh sai số nhỏ
            {
                return;
            }

            logic.reduceHealth(1); // Trừ 1 máu
        }
    }
}
