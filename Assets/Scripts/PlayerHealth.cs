﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int health = 3;
    public float invincibleTime = 1.5f;
    private bool isInvincible = false;
    private bool hasPowerUp = false;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private CheckpointSystem checkpointSystem;
    private PowerUpController powerUpController;
    private Animator animator;

    public static int playersAlive = 0;
    public bool isPlayer1 = false;

    public int Health
    {
        get { return health; }
    }

    private void Awake()
    {
        // Only reset playersAlive if this is the first scene load
        if (!PlayerPrefs.HasKey("GameStarted"))
        {
            playersAlive = 0;
            PlayerPrefs.SetInt("GameStarted", 1);
        }
    }

    private void OnDestroy()
    {
        if (gameObject.activeSelf) playersAlive--;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        checkpointSystem = GetComponent<CheckpointSystem>();
        powerUpController = GetComponent<PowerUpController>();
        animator = GetComponent<Animator>();

        if (gameObject.name.Contains("Player1"))
        {
            isPlayer1 = true;
        }

        playersAlive++;
        Debug.Log("Players alive after adding: " + playersAlive);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Fire") && collision is CapsuleCollider2D)
        {
            if (hasPowerUp)
            {
                Debug.Log("Power-Up lost, entering invincible state");
                hasPowerUp = false;
                powerUpController.DeactivatePowerUp();
                StartCoroutine(BecomeInvincible());
            }
            else if (!isInvincible)
            {
                Debug.Log("Player attacked");
                TakeDamage();
            }
        }
        else if (collision.CompareTag("KillZone"))
        {
            Debug.Log("Player fell into Killzone");
            if (hasPowerUp)
            {
                hasPowerUp = false;
                powerUpController.DeactivatePowerUp();
                Debug.Log("Power-Up lost due to Killzone");
                StartCoroutine(BecomeInvincible());
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
        if (!gameObject.activeSelf) return;

        Debug.Log(gameObject.name + " died!");
     
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 2.5f; 
        rb.linearVelocity = new Vector2(0, 10f); 

        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }

        animator.SetTrigger("isDead");
    
        gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");

        if (!isPlayer1) 
        {
            Debug.Log("[PlayerHealth] Player2 death detected. Disabling respawn...");
            FindObjectOfType<Player2Spawn>().SetPlayer2Dead();
        }

        StartCoroutine(FallOffScreen());
    }


    IEnumerator FallOffScreen()
    {
        yield return new WaitForSeconds(1f);
    
        Debug.Log("Current players alive before processing: " + playersAlive);
    
        int remainingPlayers = playersAlive - 1;
    
        if (remainingPlayers <= 0)
        {
            Debug.Log("All players are dead. Playing game over music...");
            
            AudioManager.instance.PlayGameOver();
    
            yield return new WaitForSeconds(AudioManager.instance.gameOverClip.length);
    
            Debug.Log("Game over music finished. Restarting scene...");
            FindObjectOfType<GameOverUI>().ShowGameOverScreen();
        }
        else
        {
            playersAlive = remainingPlayers;
            gameObject.SetActive(false);
            Debug.Log(gameObject.name + " is disabled, players remaining: " + playersAlive);
    
            CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
    
            if (isPlayer1 && cameraFollow != null && cameraFollow.player2 != null)
            {
                cameraFollow.player1 = cameraFollow.player2;
                cameraFollow.player2 = null;
                cameraFollow.isPlayer2Active = false;
            }
            else if (!isPlayer1 && cameraFollow != null)
            {
                cameraFollow.player2 = null;
                cameraFollow.isPlayer2Active = false;
            }
    
            AudioManager.instance.PlayBGMForCurrentScene(); // Ensure BGM plays when players are still alive
        }
    }

    public void KillZoneCheckPointRespawn()
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
