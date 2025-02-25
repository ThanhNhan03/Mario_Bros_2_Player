using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public Color player1PowerUpColor = Color.red;
    public Color player2PowerUpColor = Color.blue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PowerUpController player = other.GetComponent<PowerUpController>();

        if (player != null && !player.IsPoweredUp)
        {
            if (other.CompareTag("Player1"))
            {
                player.ActivatePowerUp(player1PowerUpColor);
            }
            else if (other.CompareTag("Player2"))
            {
                player.ActivatePowerUp(player2PowerUpColor);
            }

            Destroy(gameObject);
        }
    }
}
