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
        if (collision.CompareTag("Player1Foot") || collision.CompareTag("Player2Foot"))
        {
            PlayerMovement player = collision.GetComponentInParent<PlayerMovement>();
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