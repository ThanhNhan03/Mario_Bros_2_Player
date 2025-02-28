using System.Collections;
using UnityEngine;

public class LuckyBlock : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] spawnItems;
    public Transform spawnPoint;
    public AudioClip hitSound;

    private bool isUsed = false;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isUsed && (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2")))
        {
            if (IsHitFromBelow(collision))
            {
                ActivateBlock();
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

    private void ActivateBlock()
    {
        isUsed = true;
        animator.SetTrigger("isUsed");

        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }

        if (spawnItems.Length > 0)
        {
            GameObject itemToSpawn = spawnItems[Random.Range(0, spawnItems.Length)];

            Vector3 spawnPosition = transform.position + Vector3.up * 0.2f; 
            GameObject spawnedItem = Instantiate(itemToSpawn, spawnPosition, Quaternion.identity);

            StartCoroutine(MoveUp(spawnedItem)); 
        }
    }

    private IEnumerator MoveUp(GameObject item)
    {
        float duration = 0.5f;
        Vector3 targetPosition = item.transform.position + Vector3.up * 0.8f;
        float elapsedTime = 0f;

        Collider2D itemCollider = item.GetComponent<Collider2D>();
        if (itemCollider != null)
        {
            itemCollider.enabled = false; 
        }

        while (elapsedTime < duration)
        {
            item.transform.position = Vector3.Lerp(item.transform.position, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        item.transform.position = targetPosition;

        if (itemCollider != null)
        {
            itemCollider.enabled = true;
        }
    }


}
