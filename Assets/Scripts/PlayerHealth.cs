using UnityEngine;
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
        // Reset players alive count at the start of each scene
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            // Only reset on the first scene (main menu)
            playersAlive = 0;
            PlayerPrefs.SetInt("GameStarted", 1);
        }
        else
        {
            // For other scenes, make sure we're properly tracking players
            // This ensures we reset the count when transitioning between levels
            playersAlive = 0; // Reset counter at the start of each scene
        }
    }

    private void OnDestroy()
    {
        // Remove this decrement as we'll handle it in FallOffScreen
        // if (gameObject.activeSelf) playersAlive--;
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

        // Only increment if we're active
        if (gameObject.activeSelf)
        {
            playersAlive++;
            Debug.Log(gameObject.name + " added. Players alive: " + playersAlive);
        }
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
         
        GetComponent<PlayerMovement>().SetMovementEnabled(false);
        
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

        // Ensure we don't go below zero
        playersAlive = Mathf.Max(0, playersAlive - 1);
        
        // Check if there are any active players left in the scene
        bool player1Active = false;
        bool player2Active = false;
        
        GameObject[] player1Objects = GameObject.FindGameObjectsWithTag("Player1");
        GameObject[] player2Objects = GameObject.FindGameObjectsWithTag("Player2");
        
        foreach (GameObject player in player1Objects)
        {
            if (player.activeSelf && player != gameObject) player1Active = true;
        }
        
        foreach (GameObject player in player2Objects)
        {
            if (player.activeSelf && player != gameObject) player2Active = true;
        }
        
        // If this is player1 dying and player2 is still active, or vice versa, continue the game
        if ((isPlayer1 && player2Active) || (!isPlayer1 && player1Active))
        {
            playersAlive = 1; // Ensure we have exactly 1 player alive
            gameObject.SetActive(false);
            Debug.Log(gameObject.name + " is disabled, but other player still active");

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
        }
        // If both players are dead, show game over
        else if (!player1Active && !player2Active)
        {
            Debug.Log("All players are dead. Playing game over music...");
            
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayGameOver();
                yield return new WaitForSeconds(AudioManager.instance.gameOverClip.length);
            }
            else
            {
                yield return new WaitForSeconds(2f);
            }
    
            Debug.Log("Game over music finished. Showing game over screen...");
            GameOverUI gameOverUI = FindObjectOfType<GameOverUI>();
            if (gameOverUI != null)
            {
                gameOverUI.ShowGameOverScreen();
            }
            else
            {
                Debug.LogError("GameOverUI not found in the scene!");
                // Fallback - reload current scene after a delay
                yield return new WaitForSeconds(2f);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        // This player is the last one alive but is now dead
        else
        {
            Debug.Log("Last player has died. Playing game over music...");
            
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayGameOver();
                yield return new WaitForSeconds(AudioManager.instance.gameOverClip.length);
            }
            else
            {
                yield return new WaitForSeconds(2f);
            }
    
            Debug.Log("Game over music finished. Showing game over screen...");
            GameOverUI gameOverUI = FindObjectOfType<GameOverUI>();
            if (gameOverUI != null)
            {
                gameOverUI.ShowGameOverScreen();
            }
            else
            {
                Debug.LogError("GameOverUI not found in the scene!");
                // Fallback - reload current scene after a delay
                yield return new WaitForSeconds(2f);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
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
