using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public LogicCode logic;

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("LogicManager").GetComponent<LogicCode>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7) // Nhân vật có layer 7
        {
            logic.reduceHealth(1); // Gây sát thương - trừ 1 máu
        }
    }
}
