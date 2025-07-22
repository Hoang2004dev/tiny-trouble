using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformMover : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Rigidbody2D rb;
    private Vector3 target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = pointB.position;
    }

    void FixedUpdate()
    {
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (Vector2.Distance(rb.position, target) < 0.05f)
        {
            target = (target == pointA.position) ? pointB.position : pointA.position;
        }
    }
}
