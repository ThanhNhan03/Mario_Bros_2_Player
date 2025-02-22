using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public LayerMask bounceableLayers; // Các Layer mà đạn có thể nảy
    public GameObject explosionEffect; // Prefab hiệu ứng nổ
    public float speed = 10f;
    public float bounceForce = 8f;
    public int maxBounces = 3;

    private Rigidbody2D rb;
    private int bounceCount = 0;

    public void Initialize(Vector2 direction)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & bounceableLayers) != 0)
        {
            // Nếu đạn chạm vào bề mặt có thể nảy
            if (bounceCount < maxBounces)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
                bounceCount++;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, 0.5f); 
        }
        Destroy(gameObject);
    }
}
