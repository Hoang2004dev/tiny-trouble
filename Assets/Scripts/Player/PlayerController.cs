using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 11f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public float jumpCooldown = 0.1f;
    public int maxJumps = 2;

    public float knockbackForce = 10f;
    public float knockbackDuration = 0.2f;
    public float knockbackAngle = 45f;

    private Rigidbody2D rb;
    private Animator animator;

    private float moveInput = 0f;
    private bool jumpPressed = false;
    private float lastJumpTime = -999f;
    private int jumpCount = 0;
    private bool wasGrounded = false;
    private bool facingRight = true;
    private bool isKnockedBack = false;
    private bool canMove = true;

    [Header("Audio Settings")]
    public AudioClip jumpSound;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (PlayerInputHandler.Instance != null)
        {
            PlayerInputHandler.Instance.OnMoveChanged += HandleMove;
            PlayerInputHandler.Instance.OnJumpPressed += HandleJump;
        }
    }

    void Update()
    {
        if (!canMove) return;

        bool isGrounded = IsGrounded();

        if (isGrounded && !wasGrounded)
        {
            jumpCount = 0;
        }
        wasGrounded = isGrounded;

        if (jumpPressed && jumpCount < maxJumps && Time.time - lastJumpTime >= jumpCooldown)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            lastJumpTime = Time.time;
            jumpCount++;

            if (audioSource != null && jumpSound != null)
                audioSource.PlayOneShot(jumpSound);
        }

        jumpPressed = false;
        UpdateFacingDirection();

        // Animator update
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("IsJumpingOrFalling", !isGrounded);
        animator.SetBool("FacingRight", facingRight);
    }

    void FixedUpdate()
    {
        if (!canMove || isKnockedBack)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void HandleMove(Vector2 input)
    {
        if (!canMove)
        {
            moveInput = 0f;
            return;
        }

        moveInput = input.x;
    }

    private void HandleJump()
    {
        if (!canMove) return;
        jumpPressed = true;
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckRadius + 0.05f, groundLayer);
        bool overlap = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        return hit.collider != null || overlap;
    }

    public void ApplyKnockback(Vector2 incomingDirection)
    {
        if (isKnockedBack) return;

        isKnockedBack = true;
        rb.linearVelocity = Vector2.zero;

        Vector2 baseDir = incomingDirection.normalized;
        Vector2 knockbackDir = RotateVector(baseDir, knockbackAngle);

        rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
        Invoke(nameof(ResetKnockback), knockbackDuration);
    }

    private Vector2 RotateVector(Vector2 v, float angleDegrees)
    {
        return Quaternion.Euler(0, 0, angleDegrees) * v;
    }

    private void ResetKnockback()
    {
        isKnockedBack = false;
    }

    private void UpdateFacingDirection()
    {
        if (moveInput > 0.01f && !facingRight)
            facingRight = true;
        else if (moveInput < -0.01f && facingRight)
            facingRight = false;
    }

    public void DisableMovement()
    {
        canMove = false;
        moveInput = 0f;
        rb.linearVelocity = Vector2.zero;
        Debug.Log("[PlayerController] 🚫 Movement disabled");
    }

    public void EnableMovement()
    {
        canMove = true;
        Debug.Log("[PlayerController] ✅ Movement enabled");
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * (groundCheckRadius + 0.05f));
        }
    }
}
