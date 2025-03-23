using System.Collections;
using UnityEngine;

public class BreakableBrick : MonoBehaviour
{
    public GameObject breakEffect;
    public AudioClip breakSound;
    public Sprite emptyBlock;
    
    private bool animating;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //if (audioSource == null)
        //{
        //    Debug.LogWarning("AudioSource is missing on " + gameObject.name);
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for boss first
        if (collision.gameObject.CompareTag("Boss"))
        {
            BreakBrick();
            return;
        }

        // Existing player logic
        if (collision.gameObject.CompareTag("Player1") || (collision.gameObject.CompareTag("Player2")))
        {
            PowerUpController powerUpController = collision.gameObject.GetComponent<PowerUpController>();

            if (powerUpController != null && IsHitFromBelow(collision))
            {
                if (powerUpController.IsPoweredUp)
                {
                    BreakBrick();
                }
                else
                {
                    StartCoroutine(Animate()); 
                }
            }
        }
    }

    private bool IsHitFromBelow(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.point.y < transform.position.y)
            {
                return true;
            }
        }
        return false;
    }

    private void BreakBrick()
    {
        if (breakEffect != null)
        {
            Instantiate(breakEffect, transform.position, Quaternion.identity);
        }

        if (breakSound != null)
        {
            AudioSource.PlayClipAtPoint(breakSound, transform.position, 2f);
        }

        Destroy(gameObject);
    }

    private IEnumerator Animate()
    {
        animating = true;

        Vector3 restingPosition = transform.localPosition;
        Vector3 animatedPosition = restingPosition + Vector3.up * 0.2f;

        yield return Move(restingPosition, animatedPosition);
        yield return Move(animatedPosition, restingPosition);

        animating = false;
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = 0.1f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = to;
    }
}
