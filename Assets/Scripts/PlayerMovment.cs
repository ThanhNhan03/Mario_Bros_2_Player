using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    public bool isPlayer1;

    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float acceleration = 12f;
    public float deceleration = 10f;
    public float sprintMultiplier = 1.5f;
    public float jumpForce = 16f;
    public float sprintJumpMultiplier = 1.3f;
    public float gravityScale = 5f;

    [Header("Shooting Settings")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float shootCooldown = 0.5f;

    [Header("References")]
    public LayerMask groundLayer;
    public AudioClip jumpSFX;
    private PowerUpController powerUpController; // 🎯 Thêm biến kiểm tra PowerUp

    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;

    private float moveDirection = 0f;
    private bool isJumping = false;
    private bool isGrounded = false;
    private bool isSprinting = false;
    private bool facingRight = true;
    private float velocityX = 0f;
    private float lastShootTime = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        powerUpController = GetComponent<PowerUpController>(); // 🔥 Lấy tham chiếu PowerUpController
    }

    private void Update()
    {
        HandleInput();
        CheckGround();
        FlipCharacter();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        Jump();
    }

    private void HandleInput()
    {
        KeyCode leftKey = isPlayer1 ? KeyCode.A : KeyCode.LeftArrow;
        KeyCode rightKey = isPlayer1 ? KeyCode.D : KeyCode.RightArrow;
        KeyCode jumpKey = isPlayer1 ? KeyCode.J : KeyCode.Keypad1;
        KeyCode sprintKey = isPlayer1 ? KeyCode.K : KeyCode.Keypad2;
        KeyCode shootKey = isPlayer1 ? KeyCode.L : KeyCode.Keypad3;

        moveDirection = (Input.GetKey(leftKey) ? -1f : 0f) + (Input.GetKey(rightKey) ? 1f : 0f);
        isSprinting = Input.GetKey(sprintKey);

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            isJumping = true;
        }

        if (Input.GetKeyDown(shootKey) && Time.time >= lastShootTime + shootCooldown)
        {
            if (powerUpController != null && powerUpController.IsPoweredUp)
            {
                Shoot();
                lastShootTime = Time.time;
            }
        }
    }

    private void MovePlayer()
    {
        float finalSpeed = moveSpeed * (isSprinting && moveDirection != 0 ? sprintMultiplier : 1f);

        if (moveDirection != 0)
            velocityX = Mathf.MoveTowards(velocityX, finalSpeed * moveDirection, acceleration * Time.fixedDeltaTime);
        else
            velocityX = Mathf.MoveTowards(velocityX, 0, deceleration * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector2(velocityX, rb.linearVelocity.y);
    }

    private void Jump()
    {
        if (isJumping)
        {
            float jumpPower = isSprinting ? jumpForce * sprintJumpMultiplier : jumpForce;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);

            if (jumpSFX != null && audioSource != null)
            {
                audioSource.PlayOneShot(jumpSFX);
            }
            isJumping = false;
        }
    }

    private void CheckGround()
    {
        float extraHeight = 0.1f;
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, GetComponent<Collider2D>().bounds.extents.y + extraHeight, groundLayer);
    }

    private void FlipCharacter()
    {
        if ((moveDirection < 0 && facingRight) || (moveDirection > 0 && !facingRight))
        {
            facingRight = !facingRight;
            transform.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);
        }
    }

    private void UpdateAnimation()
    {
        if (animator == null) return;

        animator.SetBool("isJumping", !isGrounded);
        animator.SetBool("isRunning", moveDirection != 0);
        animator.speed = isSprinting ? 1.8f : 1.0f;
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<BulletMovement>().Initialize(new Vector2(facingRight ? 1 : -1, 0));
    }

    public void Bounce()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 0.7f);
    }
}
