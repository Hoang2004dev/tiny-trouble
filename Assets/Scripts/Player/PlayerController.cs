using UnityEngine;
using UnityEngine.InputSystem;

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

    private Rigidbody2D rb;
    private Animator animator;

    private float moveInput = 0f;
    private bool jumpPressed = false;
    private float lastJumpTime = -999f;

    private int jumpCount = 0;
    public int maxJumps = 2;

    private bool wasGrounded = false; // Thêm biến để theo dõi trạng thái grounded trước đó

    private enum FirstKey { None, A, D }
    private FirstKey firstKeyPressed = FirstKey.None;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool isGrounded = IsGrounded();

        // Reset jumpCount chỉ khi vừa tiếp đất (từ trên không chuyển sang grounded)
        if (isGrounded && !wasGrounded)
        {
            jumpCount = 0;
        }
        wasGrounded = isGrounded;

        // Nhảy nếu đủ điều kiện
        if (jumpPressed && jumpCount < maxJumps && Time.time - lastJumpTime >= jumpCooldown)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            lastJumpTime = Time.time;
            jumpCount++;
        }

        // Reset nhảy sau 1 frame
        jumpPressed = false;

        // Cập nhật Animator
        if (moveInput > 0)
            animator.SetBool("FacingRight", true);
        else if (moveInput < 0)
            animator.SetBool("FacingRight", false);

        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("IsJumpingOrFalling", !isGrounded);
        animator.SetBool("IsMovingRight", rb.linearVelocity.x > 0.1f);
        animator.SetBool("IsMovingLeft", rb.linearVelocity.x < -0.1f);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        bool dPressed = Keyboard.current != null && Keyboard.current.dKey.isPressed;
        bool aPressed = Keyboard.current != null && Keyboard.current.aKey.isPressed;

        if (context.started || context.performed || context.canceled)
        {
            if (firstKeyPressed == FirstKey.None)
            {
                if (dPressed && !aPressed)
                    firstKeyPressed = FirstKey.D;
                else if (aPressed && !dPressed)
                    firstKeyPressed = FirstKey.A;
            }

            if (firstKeyPressed == FirstKey.D)
            {
                if (dPressed)
                    moveInput = 1f;
                else
                {
                    firstKeyPressed = aPressed ? FirstKey.A : FirstKey.None;
                    moveInput = aPressed ? -1f : 0f;
                }
            }
            else if (firstKeyPressed == FirstKey.A)
            {
                if (aPressed)
                    moveInput = -1f;
                else
                {
                    firstKeyPressed = dPressed ? FirstKey.D : FirstKey.None;
                    moveInput = dPressed ? 1f : 0f;
                }
            }
            else
            {
                if (dPressed && !aPressed)
                {
                    firstKeyPressed = FirstKey.D;
                    moveInput = 1f;
                }
                else if (aPressed && !dPressed)
                {
                    firstKeyPressed = FirstKey.A;
                    moveInput = -1f;
                }
                else
                {
                    moveInput = 0f;
                }
            }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
