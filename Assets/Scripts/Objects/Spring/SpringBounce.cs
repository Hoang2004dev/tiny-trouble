using UnityEngine;

public class SpringBounce : MonoBehaviour
{
    [Header("Spring Settings")]
    public float bounceForce = 15f;
    public float cooldown = 0.5f;
    private float lastBounceTime = -999f;

    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra cooldown
        if (Time.time - lastBounceTime < cooldown) return;

        // Kiểm tra Player
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null && playerRb.linearVelocity.y < -0.01f)
            {
                Debug.Log($"[SpringBounce] ✅ Player đang rơi xuống (velocity.y = {playerRb.linearVelocity.y:F2})");

                // 🔸 1. Bật Player lên bằng lực
                //playerRb.AddForce(new Vector2(0f, bounceForce), ForceMode2D.Impulse);
                //Debug.Log($"[SpringBounce] 🚀 Bật Player lên với lực {bounceForce}");

                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);

                // 🔸 2. Kích hoạt animation SPRING (nếu có)
                animator?.SetBool("IsBouncing", true);

                // 🔸 3. Kích hoạt animation PLAYER Bounce theo hướng
                Animator playerAnimator = collision.GetComponent<Animator>();
                if (playerAnimator != null)
                {
                    playerAnimator.SetTrigger("Bounce");

                    // Dựa vào velocity.x hoặc localScale.x (tùy cách bạn dùng)
                    bool facingRight = playerRb.linearVelocity.x >= 0.1f;
                    playerAnimator.SetBool("FacingRight", facingRight);

                    Debug.Log($"[SpringBounce] 🎞 Gửi Trigger Bounce - Player đang hướng {(facingRight ? "Right" : "Left")}");
                }

                // 🔸 4. Cập nhật cooldown
                lastBounceTime = Time.time;
                Debug.Log($"[SpringBounce] ⏳ Cooldown bắt đầu tại {lastBounceTime:F2}");
            }
            else
            {
                Debug.Log($"[SpringBounce] ⚠️ Player không được bật (velocity.y = {playerRb?.linearVelocity.y:F2})");
            }
        }
    }

    // Gọi từ animation clip của Spring
    public void ResetBounceState()
    {
        animator?.SetBool("IsBouncing", false);
    }
}
