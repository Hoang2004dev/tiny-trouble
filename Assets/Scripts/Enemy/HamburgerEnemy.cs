using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HamburgerEnemy : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private bool movingRight = true;
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //rb.gravityScale = 0; // ko bị rơi
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // ko xoay
    }

    void FixedUpdate()      
    {
        MoveHorizontally();
    }

    void MoveHorizontally()
    {
        float moveDirection = movingRight ? 1 : -1;

        rb.linearVelocity = new Vector2(moveDirection * speed, rb.linearVelocity.y);

        if (movingRight)
        {
            animator.Play("HamburgerWalkRight");
        }
        else
        {
            animator.Play("HamburgerWalkLeft");
        }

        if (movingRight && transform.position.x >= pointB.position.x)
        {
            movingRight = false;
        }
        else if (!movingRight && transform.position.x <= pointA.position.x)
        {
            movingRight = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>()?.TakeDamage(1);
        }
    }
}
