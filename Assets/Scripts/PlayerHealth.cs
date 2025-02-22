using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;
    public float invincibleTime = 1.5f;
    private bool isInvincible = false;
    private bool hasPowerUp = false; // Trạng thái có Power-Up hay không
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private CheckpointSystem checkpointSystem;
    private PowerUpController powerUpController;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        checkpointSystem = GetComponent<CheckpointSystem>();
        powerUpController = GetComponent<PowerUpController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision is CapsuleCollider2D)
        {
            if (hasPowerUp)
            {
                Debug.Log("Power-Up lost, entering invincible state");
                hasPowerUp = false;
                powerUpController.DeactivatePowerUp(); // Tắt Power-Up
                StartCoroutine(BecomeInvincible()); // Bật Invincible khi mất Power-Up
            }
            else if (!isInvincible)
            {
                Debug.Log("Player attacked");
                TakeDamage();
            }
        }
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
        // Respawn(); // Thêm logic chết hoặc hiệu ứng nếu cần
    }

    public void Respawn()
    {
        if (checkpointSystem != null)
        {
            transform.position = checkpointSystem.GetCheckpointPosition();
            Debug.Log("Player respawned at checkpoint");
        }
        else
        {
            Debug.Log("No checkpoint set, respawning at default position");
        }
    }

    public void SetInvincible(bool value)
    {
        isInvincible = value;
        hasPowerUp = value;
    }
}
