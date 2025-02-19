using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;
    public float invincibleTime = 1.5f;
    private bool isInvincible = false;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private CheckpointSystem checkpointSystem; 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        checkpointSystem = GetComponent<CheckpointSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isInvincible && collision.CompareTag("Enemy") && collision is CapsuleCollider2D)
        {
            Debug.Log("Player attacked");
            TakeDamage();
        }
        //else if (collision.CompareTag("KillZone"))
        //{
        //    Debug.Log("Player fell into KillZone");
        //    TakeDamage();
        //    Respawn();
        //}
    }

    public void TakeDamage()
    {
        health--;
        Debug.Log("Player health: " + health);

        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(BecomeInvincible());
        }
    }

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
        Debug.Log("Player died");
        //Respawn(); // Gọi Respawn thay vì hủy nhân vật

        //dang cap nhap logic cho die
    }

    public void Respawn()
    {
        if (checkpointSystem != null)
        {
            transform.position = checkpointSystem.GetCheckpointPosition(); // Hồi sinh tại checkpoint gần nhất
            Debug.Log("Player respawned at checkpoint");
        }
        else
        {
            Debug.Log("No checkpoint set, respawning at default position");
        }
    }
}