using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Máu")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Hiển thị UI")]
    public HealthUIManager healthUI;

    [Header("Tạm thời bất tử")]
    public float invincibleDuration = 1f;
    private bool isInvincible = false;

    [Header("Tự chết nếu rơi khỏi map")]
    public float deathYThreshold = -10f;

    private Animator animator;

    public static bool isTransitioning = false;

    [Header("Âm thanh")]
    public AudioClip hurtSound;
    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

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
            healthUI.UpdateHearts(currentHealth);

        if (animator != null)
            animator.SetTrigger("Hurt");

        if (hurtSound != null && audioSource != null)
            audioSource.PlayOneShot(hurtSound);

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
                sprite.enabled = !sprite.enabled; 
                yield return new WaitForSeconds(blinkInterval);
                elapsed += blinkInterval;
            }

            sprite.enabled = true; 
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

        CinemachineCamera cam = FindFirstObjectByType<CinemachineCamera>();
        if (cam != null && cam.Follow != null)
        {
            cam.Follow = null;
        }

        if (animator != null)
            animator.SetTrigger("PlayerDie");

        GetComponent<PlayerController>().enabled = false;

        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
            col.enabled = false;

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

        Invoke(nameof(RestartLevel), 2f);
    }

    private void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
