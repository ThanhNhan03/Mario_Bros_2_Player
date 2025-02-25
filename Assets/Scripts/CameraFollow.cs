using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public bool isPlayer2Active = false;

    [Header("Camera Settings")]
    public float smoothSpeed = 5f;
    public float minZoom = 5f;
    public float maxZoom = 10f;
    public float zoomLimiter = 5f;
    public float zoomInDistance = 3f;
    public float zoomOutDistance = 6f; 

    [Header("Camera Bounds")]
    public BoxCollider2D cameraBounds;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (player1 != null && (player2 == null || !isPlayer2Active))
        {
            // Nếu chỉ có Player 1, camera luôn theo dõi Player 1
            FollowSinglePlayer(player1);
        }
        else if (player1 == null && player2 != null)
        {
            // Nếu Player 1 bị hủy, camera chuyển sang Player 2
            FollowSinglePlayer(player2);
        }
        else if (isPlayer2Active && player1 != null && player2 != null)
        {
            // Nếu cả hai Player còn tồn tại, camera sẽ theo dõi cả hai
            FollowBothPlayers();
        }

        ClampCameraPosition();
    }

    void FollowSinglePlayer(Transform targetPlayer)
    {
        if (targetPlayer == null) return;

        Vector3 targetPos = targetPlayer.position;
        targetPos.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);

        float targetZoom = Camera.main.orthographicSize;
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * smoothSpeed);
    }


    void FollowBothPlayers()
    {
        Vector3 middlePoint = (player1.position + player2.position) / 2f;
        middlePoint.z = transform.position.z;

        float distance = Vector2.Distance(player1.position, player2.position);

        // Xác định mức zoom dựa trên khoảng cách
        float targetZoom = cam.orthographicSize;

        if (distance <= zoomInDistance)
        {
            // Nếu hai người chơi gần nhau, zoom in về mức minZoom
            targetZoom = minZoom;
        }
        else if (distance >= zoomOutDistance)
        {
            // Nếu hai người chơi xa nhau, zoom out về mức maxZoom
            targetZoom = maxZoom;
        }
        else
        {
            // Nếu khoảng cách nằm giữa zoomInDistance và zoomOutDistance, tính toán mức zoom trung gian
            float t = (distance - zoomInDistance) / (zoomOutDistance - zoomInDistance);
            targetZoom = Mathf.Lerp(minZoom, maxZoom, t);
        }

        // Áp dụng mức zoom mới
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * smoothSpeed);

        // Di chuyển camera đến vị trí giữa 2 người chơi
        transform.position = Vector3.Lerp(transform.position, middlePoint, smoothSpeed * Time.deltaTime);
    }

    void ClampCameraPosition()
    {
        if (cameraBounds == null) return;

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        Bounds bounds = cameraBounds.bounds;

        float minX = bounds.min.x + camWidth;
        float maxX = bounds.max.x - camWidth;
        float minY = bounds.min.y + camHeight;
        float maxY = bounds.max.y - camHeight;

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    public void ActivatePlayer2(Transform p2)
    {
        player2 = p2;
        isPlayer2Active = true;
    }
}
