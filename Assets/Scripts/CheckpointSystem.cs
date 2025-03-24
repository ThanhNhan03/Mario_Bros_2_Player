using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    private Vector3 lastCheckpointPosition;
    private bool hasCheckpoint = false;

    private void Start()
    {
        lastCheckpointPosition = transform.position; 
    }

    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;
        hasCheckpoint = true;
        Debug.Log("Checkpoint set at: " + checkpointPosition);
    }

    public Vector3 GetCheckpointPosition()
    {
        return hasCheckpoint ? lastCheckpointPosition : transform.position;
    }
}
