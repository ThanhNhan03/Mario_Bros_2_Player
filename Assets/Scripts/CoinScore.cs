using UnityEngine;

public class CoinScore : MonoBehaviour
{
    [SerializeField] int coinScore = 100;
    [SerializeField] AudioClip coinSFX;
    private bool isCollected = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCollected && (other.CompareTag("Player1") || other.CompareTag("Player2")))
        {
            isCollected = true; 
            GameManager.instance.AddScore(other.gameObject, coinScore);
            AudioSource.PlayClipAtPoint(coinSFX, transform.position, 2f);
            Destroy(gameObject);
        }
    }
}
