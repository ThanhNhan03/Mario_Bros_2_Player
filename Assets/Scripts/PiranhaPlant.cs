using UnityEngine;
using UnityEngine.SceneManagement;

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
    private int lastSceneIndex = -1;

    [SerializeField] private float rotationAngle = 270;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset references when a new scene is loaded
        player1 = null;
        player2 = null;
        player1Health = null;
        player2Health = null;
        FindPlayers();
    }

    void Start()
    {
        FindPlayers();
        fireCooldown = fireRate;

        if (shootPoint != null)
        {
            shootPoint.SetParent(head);
        }
        
        lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void FindPlayers()
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
    }

    void Update()
    {
        // Check if scene has changed
        if (lastSceneIndex != SceneManager.GetActiveScene().buildIndex)
        {
            lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
            FindPlayers();
        }

        // Try to find players if they're null
        if (player1 == null || player1Health == null)
        {
            GameObject p1 = GameObject.FindGameObjectWithTag("Player1");
            if (p1 != null)
            {
                player1 = p1.transform;
                player1Health = p1.GetComponent<PlayerHealth>();
            }
        }

        if (player2 == null || player2Health == null)
        {
            GameObject p2 = GameObject.FindGameObjectWithTag("Player2");
            if (p2 != null)
            {
                player2 = p2.transform;
                player2Health = p2.GetComponent<PlayerHealth>();
            }
        }

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

        angle += rotationAngle;

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        head.rotation = Quaternion.Lerp(head.rotation, targetRotation, Time.deltaTime * 5f);
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
