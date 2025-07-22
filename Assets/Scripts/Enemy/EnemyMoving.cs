using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMoving : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private bool movingRight = true;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void FixedUpdate()
    {
        MoveHorizontally();
    }

    void MoveHorizontally()
    {
        float moveDirection = movingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(moveDirection * speed, rb.linearVelocity.y);

        // Flip sprite based on direction
        spriteRenderer.flipX = !movingRight;

        // Switch direction at boundaries
        if (movingRight && transform.position.x >= pointB.position.x)
        {
            movingRight = false;
        }
        else if (!movingRight && transform.position.x <= pointA.position.x)
        {
            movingRight = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>()?.TakeDamage(1);
        }
    }
}
