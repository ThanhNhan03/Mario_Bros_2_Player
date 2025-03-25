using System.Collections;
using UnityEngine;

public class BossChase : MonoBehaviour
{
    public float startSpeed = 5f; 
    public float acceleration = 0.2f; 
    public float maxSpeed = 20f;  // Maximum speed cap
    public float slowDownFactor = 0.5f; 
    public float slowDownDuration = 1f;
    public float shakeIntensity = 0.5f;
    public float shakeDuration = 0.5f;
    public AudioClip landingSound;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;
    public bool moveRight = true; // Direction of movement

    private float currentSpeed;
    private bool canMove = false;
    private bool isSlowedDown = false;
    private float slowDownTimer = 0f;
    private bool isGrounded;
    private bool wasInAir = true;
    private Rigidbody2D rb;

    void Start()
    {
        currentSpeed = startSpeed;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckGround();
        CheckLanding();

        if (!canMove) return;

        // Handle slowdown timer
        if (isSlowedDown)
        {
            slowDownTimer -= Time.deltaTime;
            if (slowDownTimer <= 0)
            {
                isSlowedDown = false;
                currentSpeed = startSpeed;
            }
        }

        float effectiveSpeed = isSlowedDown ? currentSpeed * slowDownFactor : currentSpeed;
        
        // Move in the specified direction
        float direction = moveRight ? 1 : -1;
        transform.position += new Vector3(direction * effectiveSpeed * Time.deltaTime, 0, 0);

        if (!isSlowedDown)
        {
            // Increase speed but cap at maxSpeed
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        }
    }

    void CheckGround()
    {
        isGrounded = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            GetComponent<Collider2D>().bounds.extents.y + groundCheckDistance,
            groundLayer
        );
    }

    void CheckLanding()
    {
        if (isGrounded && wasInAir)
        {
            StartCoroutine(LandingEffect());
            wasInAir = false;
        }
        else if (!isGrounded)
        {
            wasInAir = true;
        }
    }

    private IEnumerator LandingEffect()
    {
        // Play landing sound
        if (AudioManager.instance != null && landingSound != null)
        {
            AudioManager.instance.PlayOneShot(landingSound);
        }

        // Shake camera
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Vector3 originalPos = mainCamera.transform.position;
            float elapsed = 0f;

            while (elapsed < shakeDuration)
            {
                float x = Random.Range(-1f, 1f) * shakeIntensity;
                float y = Random.Range(-1f, 1f) * shakeIntensity;

                mainCamera.transform.position = new Vector3(
                    originalPos.x + x,
                    originalPos.y + y,
                    originalPos.z
                );

                elapsed += Time.deltaTime;
                yield return null;
            }

            mainCamera.transform.position = originalPos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("BreakableBlock"))
        {
            isSlowedDown = true;
            slowDownTimer = slowDownDuration;
            Debug.Log("Boss slowed down by block collision");
        }

        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                while (playerHealth.Health > 0)
                {
                    playerHealth.TakeDamage();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BossKiller"))
        {
            StartCoroutine(DestroyBoss());
        }
    }

    private IEnumerator DestroyBoss()
    {
        canMove = false;
        
        GetComponent<Collider2D>().enabled = false;
        
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayBGMForCurrentScene(); 
        }
        
        yield return new WaitForSeconds(0.5f);
        
        Destroy(gameObject);
    }

    public void StartChasing()
    {
        canMove = true;
    }
}
