using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            CheckpointSystem checkpointSystem = other.GetComponent<CheckpointSystem>();
            if (checkpointSystem != null)
            {
                checkpointSystem.SetCheckpoint(transform.position);
                Debug.Log("Checkpoint reached at: " + transform.position);
            }
        }
    }
}
