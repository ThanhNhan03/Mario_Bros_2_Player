using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    private GoobasMovement enemy;
    private Collider2D enemyHeadCollider;
    [SerializeField] private int scoreValue = 200;

    void Start()
    {
        enemy = GetComponentInParent<GoobasMovement>();
        enemyHeadCollider = GetComponent<Collider2D>();
        
        // Ignore collisions with bullets
        Physics2D.IgnoreLayerCollision(
            gameObject.layer,
            LayerMask.NameToLayer("Bullet"),
            true
        );
        
        // Ignore collisions with boss
        Physics2D.IgnoreLayerCollision(
            gameObject.layer,
            LayerMask.NameToLayer("Boss"),
            true
        );
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

                GameManager.instance.AddScore(player.gameObject, scoreValue);
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