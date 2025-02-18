using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    private GoobasMovement enemy;
    private Collider2D enemyHeadCollider;

    void Start()
    {
        enemy = GetComponentInParent<GoobasMovement>();
        enemyHeadCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.Bounce();
                enemy.Die();
                DisableHeadCollider();
            }
        }
    }


    private void DisableHeadCollider()
    {
        if (enemyHeadCollider != null)
        {
            enemyHeadCollider.enabled = false;
        }
    }
}
