using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform PointA;
    public Transform PointB;
    public float Speed = 2.0f;

    private Vector3 nextPosition;

    void Start()
    {
        nextPosition = PointB.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, Speed * Time.deltaTime);

        if (transform.position == PointA.position)
        {
            nextPosition = PointB.position;
        }
        else if (transform.position == PointB.position)
        {
            nextPosition = PointA.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1") ||
            collision.gameObject.CompareTag("Player2") ||
            collision.gameObject.CompareTag("Enemy"))  
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Player1") ||
             collision.gameObject.CompareTag("Player2") ||
             collision.gameObject.CompareTag("Enemy")) &&
             collision.transform.parent == transform)
        {
            collision.transform.SetParent(null);
        }
    }
}
