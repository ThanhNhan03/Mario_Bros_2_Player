using UnityEngine;

public class Player2Spawn : MonoBehaviour
{
    public GameObject player2;
    public float spawnOffset = 1.5f;
    private GameObject player1;
    private CameraFollow cameraFollow;
    private bool player2HasDied = false;
    // Static variable to track if Player 2 should be spawned in the next scene
    public static bool shouldSpawnPlayer2 = false;

    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        cameraFollow = Camera.main.GetComponent<CameraFollow>();

        // Check if Player 2 should be spawned
        if (shouldSpawnPlayer2)
        {
            ActivatePlayer2();
        }
        else
        {
            player2.SetActive(false);
            Debug.Log("[Player2Spawn] Initialized. Player2 is inactive.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            if (player2HasDied)
            {
                Debug.Log("[Player2Spawn] Cannot spawn Player2. Player2 has already died!");
            }
            else if (player2.activeSelf)
            {
                Debug.Log("[Player2Spawn] Player2 is already active!");
            }
            else
            {
                Debug.Log("[Player2Spawn] Spawning Player2...");
                ActivatePlayer2();
            }
        }
    }

    void ActivatePlayer2()
    {
        if (player1 == null)
        {
            Debug.LogError("[Player2Spawn] ERROR: Player1 not found!");
            return;
        }

        Vector3 spawnPosition = player1.transform.position + new Vector3(spawnOffset, 0, 0);
        player2.transform.position = spawnPosition;
        player2.SetActive(true);
        cameraFollow.ActivatePlayer2(player2.transform);

        Debug.Log("[Player2Spawn] Player2 spawned at position: " + spawnPosition);
    }

    public void SetPlayer2Dead()
    {
        player2HasDied = true;
        Debug.Log("[Player2Spawn] Player2 has died. Respawn is disabled.");
    }
}
