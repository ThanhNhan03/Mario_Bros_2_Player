using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3; 
    //public float knockbackForce = 5f; 
    public float invincibleTime = 1.5f; 
    private bool isInvincible = false;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isInvincible && collision.CompareTag("Enemy") && collision is CapsuleCollider2D)
        {
            Debug.Log("Player attacked");
            TakeDamage(collision.transform.position);
        }
    }

    void TakeDamage(Vector2 enemyPosition)
    {
        health--; 
        Debug.Log("player health" + health);

        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(BecomeInvincible());
            //Knockback(enemyPosition);
        }
    }

    //void Knockback(Vector2 enemyPosition)
    //{
    //    Vector2 knockbackDirection = (transform.position - (Vector3)enemyPosition).normalized;

    //    Vector2 knockbackForceDirection = new Vector2(knockbackDirection.x * knockbackForce, 0f); 

    //    rb.AddForce(knockbackForceDirection, ForceMode2D.Impulse); 
    //}



    IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        float blinkInterval = 0.1f; 
        for (float i = 0; i < invincibleTime; i += blinkInterval)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
        spriteRenderer.enabled = true;
        isInvincible = false;
    }

    void Die()
    {
        Debug.Log("player died");
        //Destroy(gameObject);
    }
}
