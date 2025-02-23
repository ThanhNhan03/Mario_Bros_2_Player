using UnityEngine;

public class GoobasMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float groundCheckDistance = 1f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float timeToDie = 0.5f;
    [SerializeField] AudioClip DieSFX;
    [SerializeField] int scoreByGetShot = 100;

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
        if (collision.CompareTag("Bullet"))
        {
            BulletMovement bullet = collision.GetComponent<BulletMovement>();
            if (bullet != null)
            {
                GameObject shooter = bullet.GetShooter();
                if (shooter != null)
                {
                    GameManager.instance.AddScore(shooter, scoreByGetShot); 
                }
            }

            GetShot();
            Destroy(collision.gameObject);
        }

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
        isDead = true;

        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = false;

        transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);

        animator.SetTrigger("Die");

        if (DieSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(DieSFX);
        }

        Destroy(gameObject, timeToDie);
    }

    void GetShot()
    {
        isDead = true;

        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2f;

        transform.rotation = Quaternion.Euler(0, 0, 180);

        float randomDirection = Random.Range(-1f, 1f);
        rb.AddForce(new Vector2(randomDirection * 2f, 8f), ForceMode2D.Impulse);
        rb.angularVelocity = 500f;
        if (DieSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(DieSFX);
        }

        Destroy(gameObject, 2f);
    }

}
