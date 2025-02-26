using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private float respawnHeightOffset = 1.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            CheckpointSystem checkpointSystem = other.GetComponent<CheckpointSystem>();
            if (checkpointSystem != null)
            {
                Vector3 respawnPosition = transform.position + Vector3.up * respawnHeightOffset;
                checkpointSystem.SetCheckpoint(respawnPosition);
                Debug.Log(other.tag + " reached checkpoint at: " + transform.position);
                Debug.Log("Respawn position set at: " + respawnPosition);
            }
        }
    }
}
