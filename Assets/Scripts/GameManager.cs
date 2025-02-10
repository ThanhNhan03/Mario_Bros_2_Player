using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player2Prefab; // Prefab Player 2
    public Transform spawnPoint2;    // Vị trí spawn P2
    private GameObject player1;
    private GameObject player2;
    private CameraFollow cameraFollow;

    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1"); 
        cameraFollow = Camera.main.GetComponent<CameraFollow>(); 
    }

    void Update()
    {
        // Nếu P2 chưa tham gia, nhấn Enter hoặc Keypad0 để Spawn P2
        if (player2 == null && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Keypad0)))
        {
            SpawnPlayer2();
        }
    }
    void SpawnPlayer2()
    {
        if (player1 == null) return; // Đảm bảo player1 tồn tại

        // Spawn Player 2 tại vị trí hiện tại của Player 1
        player2 = Instantiate(player2Prefab, player1.transform.position, Quaternion.identity);
        player2.GetComponent<PlayerMovement>().isPlayer1 = false; // Đánh dấu là P2

        cameraFollow.ActivatePlayer2(player2.transform);
    }

}
