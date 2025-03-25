using UnityEngine;
using System.Collections;

public class BridgeSwitch : MonoBehaviour
{
    public Transform bridge;        // Tham chiếu đến cầu
    public Transform retractPoint;  // Điểm cầu sẽ rút về
    public float retractSpeed = 2f; // Tốc độ rút cầu

    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1") && !isActivated)
        {
            isActivated = true;
            StartCoroutine(RetractBridge());
        }
    }

    private IEnumerator RetractBridge()
    {
        // Rút cầu về điểm `retractPoint`
        while (Vector2.Distance(bridge.position, retractPoint.position) > 0.1f)
        {
            bridge.position = Vector2.Lerp(bridge.position, retractPoint.position, Time.deltaTime * retractSpeed);
            yield return null;
        }

        // Đảm bảo cầu đúng vị trí
        bridge.position = retractPoint.position;
    }
}
