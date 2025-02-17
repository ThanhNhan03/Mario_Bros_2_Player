using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isPlayer1;

    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float acceleration = 12f;
    public float deceleration = 10f;
    public float sprintMultiplier = 1.5f;
    public float jumpForce = 16f;
    public float sprintJumpMultiplier = 1.3f;
    public float gravityScale = 5f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private float moveDirection = 0f;
    private bool isJumping = false;
    private bool isGrounded = false;
    private float velocityX = 0f;
    private bool isSprinting = false;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        KeyCode leftKey = isPlayer1 ? KeyCode.A : KeyCode.LeftArrow;
        KeyCode rightKey = isPlayer1 ? KeyCode.D : KeyCode.RightArrow;
        KeyCode jumpKey = isPlayer1 ? KeyCode.J : KeyCode.Keypad1;
        KeyCode sprintKey = isPlayer1 ? KeyCode.K : KeyCode.Keypad2;

        // Xác định hướng di chuyển
        if (Input.GetKey(leftKey)) moveDirection = -1f;
        else if (Input.GetKey(rightKey)) moveDirection = 1f;
        else moveDirection = 0f;

        // Kiểm tra giữ nút chạy nhanh
        isSprinting = Input.GetKey(sprintKey);

        // Kiểm tra nhảy
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            isJumping = true;
        }

        CheckGround();
        FlipCharacter();
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        float finalSpeed = moveSpeed * (isSprinting ? sprintMultiplier : 1f);

        if (moveDirection != 0)
            velocityX = Mathf.MoveTowards(velocityX, finalSpeed * moveDirection, acceleration * Time.fixedDeltaTime);
        else
            velocityX = Mathf.MoveTowards(velocityX, 0, deceleration * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector2(velocityX, rb.linearVelocity.y);

        if (isJumping)
        {
            float jumpPower = isSprinting ? jumpForce * sprintJumpMultiplier : jumpForce;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isJumping = false;
        }
    }

    void CheckGround()
    {
        float extraHeight = 0.1f;
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, GetComponent<Collider2D>().bounds.extents.y + extraHeight, groundLayer);
    }

    void FlipCharacter()
    {
        if (moveDirection < 0 && facingRight)
        {
            facingRight = false;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveDirection > 0 && !facingRight)
        {
            facingRight = true;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void UpdateAnimation()
    {
        if (animator != null)
        {
            if (!isGrounded)
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isRunning", false);
            }
            else if (moveDirection != 0)
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isRunning", true);
                animator.speed = isSprinting ? 1.8f : 1.0f;
            }
            else
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isRunning", false);
                animator.speed = 1.0f;
            }
        }
    }

    public void Bounce()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 0.7f);
    }
}
