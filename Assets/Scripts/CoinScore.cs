using UnityEngine;

public class CoinScore : MonoBehaviour
{
    public LogicCode logic;

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("LogicManager").GetComponent<LogicCode>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7) // Kiểm tra nếu nhân vật chạm vào
        {
            logic.addScore(1);  // Tăng điểm số
            Destroy(gameObject); // Xóa vật thể (coin)
        }
    }
}
