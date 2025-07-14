using UnityEngine;

public class SpringBounce : MonoBehaviour
{
    [Header("Spring Settings")]
    public float bounceForce = 15f;
    public float cooldown = 0.5f;
    private float lastBounceTime = -999f;

    private Animator animator;

    [Header("Sound Settings")]
    public AudioClip boingSound;
    private AudioSource audioSource;

    private void OnEnable()
    {
        PlayerController.OnPlayerLanded += ResetBounceState;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerLanded -= ResetBounceState;
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Time.time - lastBounceTime < cooldown) return;

        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null && playerRb.linearVelocity.y < -0.01f)
            {
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);

                animator?.SetBool("IsBouncing", true);

                if (audioSource != null && boingSound != null)
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(boingSound);
                }

                Animator playerAnimator = collision.GetComponent<Animator>();
                if (playerAnimator != null)
                {
                    playerAnimator.SetTrigger("Bounce");

                    bool facingRight = playerRb.linearVelocity.x >= 0.1f;
                    playerAnimator.SetBool("FacingRight", facingRight);
                }

                lastBounceTime = Time.time;
            }
            else
            {
                Debug.Log($"[SpringBounce] Player không được bật (velocity.y = {playerRb?.linearVelocity.y:F2})");
            }
        }
    }

    public void ResetBounceState()
    {
        animator?.SetBool("IsBouncing", false);
    }
}
