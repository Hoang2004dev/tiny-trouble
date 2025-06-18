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

        // ✅ Tính hướng bay từ đầu
        Vector3 dir = (targetPosition - transform.position).normalized;
        transform.right = dir; // Quay hướng sprite theo đường bay
    }

    void Update()
    {
        if (!hasTarget) return;

        // Bay thẳng tới target
        transform.position += transform.right * speed * Time.deltaTime;

        // Đến gần mục tiêu thì tự hủy
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
                // ✅ Lấy hướng bay của đạn để knockback
                Vector2 knockDir = transform.right; // vì bạn đã set transform.right theo hướng bay

                pc.ApplyKnockback(knockDir);
            }

            Destroy(gameObject);
        }

        //if (other.CompareTag("Wall"))
        //{
        //    Destroy(gameObject);
        //}
    }
}
