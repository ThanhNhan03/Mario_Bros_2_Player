using UnityEngine;

public class GoobasMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float groundCheckDistance = 1f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float timeToDie = 0.5f;
    [SerializeField] AudioClip DieSFX;


    private Rigidbody2D rb;
    private Animator animator;
    private bool isDead = false;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isDead) return;

        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);

        if (!IsGroundInFront())
        {
            moveSpeed = -moveSpeed;
            FlipEnemyFacing();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead)
        {
            moveSpeed = -moveSpeed;
            FlipEnemyFacing();
        }
    }

    void FlipEnemyFacing()
    {
        Vector3 newScale = transform.localScale;
        newScale.x = Mathf.Abs(newScale.x) * Mathf.Sign(moveSpeed);
        transform.localScale = newScale;
    }

    bool IsGroundInFront()
    {
        Vector2 origin = new Vector2(transform.position.x + Mathf.Sign(moveSpeed) * 0.5f, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);

        Debug.DrawRay(origin, Vector2.down * groundCheckDistance, Color.red);

        return hit.collider != null;
    }

    public void Die()
    {
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        // Dừng chuyển động của enemy (đặt tốc độ về 0)
        rb.linearVelocity = Vector2.zero;


        rb.bodyType = RigidbodyType2D.Kinematic;

        rb.simulated = false;



        // Đặt enemy xuống sát mặt đất
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);

        // Chơi animation chết
        animator.SetTrigger("Die");

        if (DieSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(DieSFX);
        }


        // Hủy enemy sau thời gian delay
        Destroy(gameObject, timeToDie);
    }




}