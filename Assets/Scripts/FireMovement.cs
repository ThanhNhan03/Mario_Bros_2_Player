using UnityEngine;

public class FireMovement : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    public void Initialize(Vector2 direction)
    {
        rb = GetComponent<Rigidbody2D>();
        moveDirection = direction.normalized * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveDirection;
    }
}
