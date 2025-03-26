using UnityEngine;
using System.Collections;

public class BridgeSwitch : MonoBehaviour
{
    public Transform bridge;       
    public Transform retractPoint;  
    public float retractSpeed = 2f;
    public GameObject deadzone;

    private bool isActivated = false;

    private void Start()
    {
        // Find and disable deadzone at start
        deadzone = GameObject.FindGameObjectWithTag("deadzone");
        if (deadzone != null)
        {
            deadzone.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2") && !isActivated)
        {
            isActivated = true;
            StartCoroutine(RetractBridge());
            // Activate deadzone
            if (deadzone != null)
            {
                deadzone.SetActive(true);
            }
        }
    }

    private IEnumerator RetractBridge()
    {
        
        while (Vector2.Distance(bridge.position, retractPoint.position) > 0.1f)
        {
            bridge.position = Vector2.Lerp(bridge.position, retractPoint.position, Time.deltaTime * retractSpeed);
            yield return null;
        }

        bridge.position = retractPoint.position;
    }
}
