using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player1;
    public Transform player2; // Player 2 sẽ xuất hiện sau
    public bool isPlayer2Active = false; // Ban đầu P2 chưa có

    [Header("Camera Settings")]
    public float smoothSpeed = 5f; // Tốc độ di chuyển mượt mà
    public float minZoom = 5f; // Zoom gần nhất
    public float maxZoom = 10f; // Zoom xa nhất
    public float zoomLimiter = 5f; // Giới hạn zoom

    [Header("Camera Bounds")]
    public BoxCollider2D cameraBounds; // BoxCollider2D để xác định giới hạn

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (!isPlayer2Active || player2 == null)
        {
            Vector3 targetPos = player1.position;
            targetPos.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
        }
        else
        {
            FollowBothPlayers();
        }

        ClampCameraPosition();
    }

    void FollowBothPlayers()
    {
        Vector3 middlePoint = (player1.position + player2.position) / 2f;
        middlePoint.z = transform.position.z;

        transform.position = Vector3.Lerp(transform.position, middlePoint, smoothSpeed * Time.deltaTime);

        float distance = Vector2.Distance(player1.position, player2.position);
        float newZoom = Mathf.Lerp(maxZoom, minZoom, distance / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime * smoothSpeed);
    }

    void ClampCameraPosition()
    {
        if (cameraBounds == null)
            return;

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