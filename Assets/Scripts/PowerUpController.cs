using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private PlayerHealth playerHealth;
    private bool isPoweredUp = false;

    public bool IsPoweredUp => isPoweredUp; 
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void ActivatePowerUp(Color newColor)
    {
        if (!isPoweredUp)
        {
            isPoweredUp = true;
            spriteRenderer.color = newColor;
            playerHealth.SetInvincible(true);
        }
    }

    public void DeactivatePowerUp()
    {
        if (isPoweredUp)
        {
            isPoweredUp = false;
            spriteRenderer.color = originalColor;
            playerHealth.SetInvincible(false);
        }
    }
}
