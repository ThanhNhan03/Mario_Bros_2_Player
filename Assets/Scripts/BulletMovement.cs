using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public LayerMask bounceableLayers;
    public GameObject explosionEffect;
    public float speed = 10f;
    public float bounceForce = 8f;
    public int maxBounces = 3;

    private Rigidbody2D rb;
    private int bounceCount = 0;
    private GameObject shooter; 

    public void Initialize(Vector2 direction, GameObject shooter)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * speed;
        this.shooter = shooter; 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & bounceableLayers) != 0)
        {
            if (bounceCount < maxBounces)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
                bounceCount++;
            }
            else
            {
                Explode();
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

    public GameObject GetShooter() => shooter;
}
