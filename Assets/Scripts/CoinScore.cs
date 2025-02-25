using UnityEngine;

public class CoinScore : MonoBehaviour
{
    [SerializeField] int coinScore = 100;
    private bool isCollected = false; 

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCollected && (other.CompareTag("Player1") || other.CompareTag("Player2")))
        {
            isCollected = true; 
            GameManager.instance.AddScore(other.gameObject, coinScore); 
            Destroy(gameObject);
        }
    }
}
