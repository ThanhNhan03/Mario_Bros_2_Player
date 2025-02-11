using UnityEngine;

public class GoobasMovment : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float groundCheckDistance = 1f;
    [SerializeField] LayerMask groundLayer;
    Rigidbody2D rigidbody2d;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        rigidbody2d.linearVelocity = new Vector2(moveSpeed, rigidbody2d.linearVelocity.y);


        if (!IsGroundInFront())
        {

            moveSpeed = -moveSpeed;
            FlipEnemyFacing();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
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
}
