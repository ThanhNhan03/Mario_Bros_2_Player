using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private PlayerHealth playerHealth;
    private bool isPoweredUp = false;

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
            spriteRenderer.color = newColor; // Đổi màu Power-Up
            playerHealth.SetInvincible(true); // Bật chế độ bất tử
        }
    }

    public void DeactivatePowerUp()
    {
        if (isPoweredUp)
        {
            isPoweredUp = false;
            spriteRenderer.color = originalColor; // Trả về màu ban đầu
            playerHealth.SetInvincible(false); // Tắt chế độ bất tử
        }
    }
}
