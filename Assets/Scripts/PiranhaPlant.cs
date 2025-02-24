using UnityEngine;

public class PiranhaPlant : MonoBehaviour
{
    public Transform head;
    public Transform shootPoint; // Giữ nguyên, nhưng đảm bảo nó là con của head trong Unity
    public GameObject bulletPrefab; // Prefab viên đạn
    private Transform player1, player2;
    private Transform targetPlayer;
    public float detectionRange = 5f;
    public float fireRate = 2f; // Thời gian giữa các lần bắn
    private float fireCooldown;

    [SerializeField] private float rotationAngle = 270;

    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1")?.transform;
        player2 = GameObject.FindGameObjectWithTag("Player2")?.transform;
        fireCooldown = fireRate;

        // Quan trọng: Đảm bảo shootPoint là con của head
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
            }
        }

        // Lấy player gần nhất
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

        // Tạo viên đạn tại vị trí shootPoint với góc quay đúng của đầu
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        FireMovement bulletMover = bullet.GetComponent<FireMovement>();
        Debug.Log($"Head Rotation: {head.rotation.eulerAngles.z}, Head Right: {head.right}");

        bulletMover.Initialize(Quaternion.Euler(0, 0, 90) * head.right);

    }


    Transform GetNearestPlayer()
    {
        bool player1InRange = player1 != null && Vector3.Distance(player1.position, transform.position) <= detectionRange;
        bool player2InRange = player2 != null && Vector3.Distance(player2.position, transform.position) <= detectionRange;

        if (!player1InRange && !player2InRange) return null;

        float distance1 = player1InRange ? Vector3.Distance(player1.position, transform.position) : float.MaxValue;
        float distance2 = player2InRange ? Vector3.Distance(player2.position, transform.position) : float.MaxValue;

        return (distance1 < distance2) ? player1 : player2;
    }

    bool IsPlayerInRange(Transform player)
    {
        return Vector3.Distance(player.position, transform.position) <= detectionRange;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

}
