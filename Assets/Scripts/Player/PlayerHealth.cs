using Unity.Cinemachine;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public float invincibleDuration = 1f;
    private bool isInvincible = false;

    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHealth -= amount;
        Debug.Log("Player HP: " + currentHealth);

        if (animator != null)
            animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(BecomeTemporarilyInvincible());
        }
    }

    private System.Collections.IEnumerator BecomeTemporarilyInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }

    private void Die()
    {
        Debug.Log("Player Died!");

        // 🔴 Ngắt camera follow
        CinemachineCamera cam = FindFirstObjectByType<CinemachineCamera>();
        if (cam != null && cam.Follow != null)
        {
            cam.Follow = null;
            Debug.Log("[Camera] ✅ Dừng follow Player sau khi chết");
        }

        // 1. Gọi animation Die
        if (animator != null)
            animator.SetTrigger("PlayerDie");

        // 2. Vô hiệu hóa input và di chuyển
        GetComponent<PlayerController>().enabled = false;

        // 3. Tắt tất cả collider (kể cả con)
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
            col.enabled = false;

        // 4. Tạo lực đẩy ngẫu nhiên
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;

            // 👉 Lực đẩy ngang + nhẹ lên trên
            float randomX = Random.Range(-1f, 1f);
            float upwardY = Random.Range(1f, 2f);
            Vector2 knockbackForce = new Vector2(randomX, upwardY).normalized * 8f;
            rb.AddForce(knockbackForce, ForceMode2D.Impulse);

            // 🔄 4.1 Cho phép xoay và thêm xoay ngẫu nhiên
            rb.freezeRotation = false; // phải mở xoay trước
            rb.angularVelocity = Random.Range(-360f, 360f); // xoay từ trái qua phải hoặc ngược lại

            // (Tuỳ chọn) rơi nhanh hơn
            rb.gravityScale = 1.5f;
        }

        // 5. Gọi restart sau vài giây
        Invoke(nameof(RestartLevel), 2f);
    }

    private void RestartLevel()
    {
        // Gọi lại màn chơi (hoặc load scene game over)
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

}
