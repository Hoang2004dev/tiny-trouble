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
    public float jumpCooldown = 0.5f;

    private Rigidbody2D rb;
    private Animator animator;

    private float moveInput = 0f;
    private bool jumpPressed = false;
    private float lastJumpTime = -999f;

    // Biến để theo dõi phím nhấn đầu tiên
    private enum FirstKey { None, A, D }
    private FirstKey firstKeyPressed = FirstKey.None;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Nhảy nếu đủ điều kiện
        if (jumpPressed && IsGrounded() && Time.time - lastJumpTime >= jumpCooldown)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            lastJumpTime = Time.time;
        }

        // Reset nhảy sau 1 frame
        jumpPressed = false;

        // Cập nhật Animator
        if (moveInput > 0)
            animator.SetBool("FacingRight", true);
        else if (moveInput < 0)
            animator.SetBool("FacingRight", false);

        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("IsJumpingOrFalling", !IsGrounded());
        animator.SetBool("IsMovingRight", rb.linearVelocity.x > 0.1f);
        animator.SetBool("IsMovingLeft", rb.linearVelocity.x < -0.1f);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Lấy giá trị từ 2D Vector Composite
        Vector2 input = context.ReadValue<Vector2>();

        // Kiểm tra trạng thái phím
        bool dPressed = Keyboard.current != null && Keyboard.current.dKey.isPressed;
        bool aPressed = Keyboard.current != null && Keyboard.current.aKey.isPressed;

        // Xử lý logic ưu tiên phím nhấn trước
        if (context.started || context.performed || context.canceled)
        {
            // Cập nhật phím nhấn đầu tiên
            if (firstKeyPressed == FirstKey.None)
            {
                if (dPressed && !aPressed)
                    firstKeyPressed = FirstKey.D;
                else if (aPressed && !dPressed)
                    firstKeyPressed = FirstKey.A;
            }

            // Xác định moveInput dựa trên phím nhấn đầu tiên
            if (firstKeyPressed == FirstKey.D)
            {
                if (dPressed)
                    moveInput = 1f; // Ưu tiên D -> di chuyển phải
                else
                {
                    firstKeyPressed = aPressed ? FirstKey.A : FirstKey.None;
                    moveInput = aPressed ? -1f : 0f; // Chuyển sang A nếu A còn nhấn, hoặc dừng
                }
            }
            else if (firstKeyPressed == FirstKey.A)
            {
                if (aPressed)
                    moveInput = -1f; // Ưu tiên A -> di chuyển trái
                else
                {
                    firstKeyPressed = dPressed ? FirstKey.D : FirstKey.None;
                    moveInput = dPressed ? 1f : 0f; // Chuyển sang D nếu D còn nhấn, hoặc dừng
                }
            }
            else // firstKeyPressed == FirstKey.None
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
                    moveInput = 0f; // Không phím nào hoặc cả hai đều nhấn khi không có phím ưu tiên
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