using UnityEngine;

public class PiranhaPlant : MonoBehaviour
{
    public Transform head;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    private Transform player1, player2;
    private PlayerHealth player1Health, player2Health;
    private Transform targetPlayer;
    public float detectionRange = 5f;
    public float fireRate = 2f;
    private float fireCooldown;

    [SerializeField] private float rotationAngle = 270;

    void Start()
    {
        GameObject p1 = GameObject.FindGameObjectWithTag("Player1");
        GameObject p2 = GameObject.FindGameObjectWithTag("Player2");

        if (p1 != null)
        {
            player1 = p1.transform;
            player1Health = p1.GetComponent<PlayerHealth>();
        }
        if (p2 != null)
        {
            player2 = p2.transform;
            player2Health = p2.GetComponent<PlayerHealth>();
        }

        fireCooldown = fireRate;

        if (shootPoint != null)
        {
            shootPoint.SetParent(head);
        }
    }

    void Update()
    {
        if (player2 == null)
        {
            GameObject player2Object = GameObject.FindGameObjectWithTag("Player2");
            if (player2Object != null)
            {
                player2 = player2Object.transform;
                player2Health = player2Object.GetComponent<PlayerHealth>();
            }
        }

        // Kiểm tra nếu tất cả người chơi đều đã chết
        if (PlayerHealth.playersAlive <= 0) return;

        targetPlayer = GetNearestPlayer();

        if (targetPlayer != null && IsPlayerInRange(targetPlayer))
        {
            RotateHead();
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                Shoot();
                fireCooldown = fireRate;
            }
        }
    }

    void RotateHead()
    {
        if (targetPlayer == null || head == null) return;

        Vector3 direction = targetPlayer.position - head.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float snappedAngle = Mathf.Round(angle / 45) * 45;
        float offsetAngle = rotationAngle;

        head.rotation = Quaternion.Euler(0, 0, snappedAngle + offsetAngle);
    }

    void Shoot()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        FireMovement bulletMover = bullet.GetComponent<FireMovement>();

        bulletMover.Initialize(Quaternion.Euler(0, 0, 90) * head.right);
    }

    Transform GetNearestPlayer()
    {
        bool isPlayer1Alive = player1 != null && player1Health != null && player1.gameObject.activeSelf;
        bool isPlayer2Alive = player2 != null && player2Health != null && player2.gameObject.activeSelf;

        bool player1InRange = isPlayer1Alive && Vector3.Distance(player1.position, transform.position) <= detectionRange;
        bool player2InRange = isPlayer2Alive && Vector3.Distance(player2.position, transform.position) <= detectionRange;

        if (!player1InRange && !player2InRange) return null;

        float distance1 = player1InRange ? Vector3.Distance(player1.position, transform.position) : float.MaxValue;
        float distance2 = player2InRange ? Vector3.Distance(player2.position, transform.position) : float.MaxValue;

        return (distance1 < distance2) ? player1 : player2;
    }

    bool IsPlayerInRange(Transform player)
    {
        return player != null && Vector3.Distance(player.position, transform.position) <= detectionRange;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
