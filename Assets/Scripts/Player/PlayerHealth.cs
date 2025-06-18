using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Máu")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Hiển thị UI")]
    public HealthUIManager healthUI; // Kéo từ Inspector

    [Header("Tạm thời bất tử")]
    public float invincibleDuration = 1f;
    private bool isInvincible = false;

    [Header("Tự chết nếu rơi khỏi map")]
    public float deathYThreshold = -10f;

    private Animator animator;

    public static bool isTransitioning = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        if (healthUI != null)
        {
            healthUI.SetupHearts(maxHealth);
            healthUI.UpdateHearts(currentHealth);
        }
    }

    void Update()
    {
        if (isTransitioning) return;

        if (transform.position.y < deathYThreshold && currentHealth > 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Player HP: " + currentHealth);

        if (healthUI != null)
            healthUI.UpdateHearts(currentHealth); // 🔁 Cập nhật UI trái tim

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

    private IEnumerator BecomeTemporarilyInvincible()
    {
        isInvincible = true;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            float elapsed = 0f;
            float blinkInterval = 0.15f;

            while (elapsed < invincibleDuration)
            {
                sprite.enabled = !sprite.enabled; // Nhấp nháy
                yield return new WaitForSeconds(blinkInterval);
                elapsed += blinkInterval;
            }

            sprite.enabled = true; // Bật lại chắc chắn
        }
        else
        {
            yield return new WaitForSeconds(invincibleDuration);
        }

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

            float randomX = Random.Range(-1f, 1f);
            float upwardY = Random.Range(1f, 2f);
            Vector2 knockbackForce = new Vector2(randomX, upwardY).normalized * 8f;
            rb.AddForce(knockbackForce, ForceMode2D.Impulse);

            rb.freezeRotation = false;
            rb.angularVelocity = Random.Range(-360f, 360f);
            rb.gravityScale = 1.5f;
        }

        // 5. Gọi restart sau vài giây
        Invoke(nameof(RestartLevel), 2f);
    }

    private void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
