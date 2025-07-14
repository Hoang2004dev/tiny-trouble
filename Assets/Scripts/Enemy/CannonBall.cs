using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 targetPosition;
    private bool hasTarget = false;

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
        hasTarget = true;

        Vector3 dir = (targetPosition - transform.position).normalized;
        transform.right = dir;
    }

    void Update()
    {
        if (!hasTarget) return;

        transform.position += transform.right * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                Vector2 knockDir = transform.right; 

                pc.ApplyKnockback(knockDir);
            }

            Destroy(gameObject);
        }

        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
