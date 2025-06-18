using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerInputHandler Instance { get; private set; }

    private PlayerInputActions inputActions;

    // ==== Sự kiện để các script khác đăng ký ====
    public delegate void InteractAction();
    public event InteractAction OnInteractPressed;

    public delegate void JumpAction();
    public event JumpAction OnJumpPressed;

    public delegate void MoveAction(Vector2 direction);
    public event MoveAction OnMoveChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        inputActions = new PlayerInputActions();
        Debug.Log("[InputHandler] ✅ InputActions đã khởi tạo");
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        SubscribeInputEvents();
        Debug.Log("[InputHandler] ✅ Đã bật input và đăng ký sự kiện");
    }

    private void OnDisable()
    {
        if (Instance == this && inputActions != null)
        {
            UnsubscribeInputEvents();
            inputActions.Player.Disable();
            Debug.Log("[InputHandler] ❎ Đã tắt input và huỷ sự kiện");
        }
    }

    public void DisableInput()
    {
        this.enabled = false; // Sẽ tự gọi OnDisable()
        Debug.Log("[InputHandler] 🚫 Input đã bị tắt");
    }

    public void EnableInput()
    {
        this.enabled = true; // Sẽ tự gọi OnEnable()
        Debug.Log("[InputHandler] ✅ Input đã được bật lại");
    }

    private void SubscribeInputEvents()
    {
        inputActions.Player.Interact.performed += Interact_performed;
        inputActions.Player.Jump.performed += Jump_performed;
        inputActions.Player.Move.performed += Move_performed;
        inputActions.Player.Move.canceled += Move_canceled;

        Debug.Log("[InputHandler] 🔗 Đã đăng ký Interact, Jump và Move");
    }

    private void UnsubscribeInputEvents()
    {
        inputActions.Player.Interact.performed -= Interact_performed;
        inputActions.Player.Jump.performed -= Jump_performed;
        inputActions.Player.Move.performed -= Move_performed;
        inputActions.Player.Move.canceled -= Move_canceled;

        Debug.Log("[InputHandler] 🔓 Đã gỡ đăng ký Interact, Jump và Move");
    }

    private void Interact_performed(InputAction.CallbackContext context)
    {
        Debug.Log("[InputHandler] 🟢 Interact (Enter/E) được nhấn");
        OnInteractPressed?.Invoke();
    }

    private void Jump_performed(InputAction.CallbackContext context)
    {
        Debug.Log("[InputHandler] 🟡 Jump được nhấn");
        OnJumpPressed?.Invoke();
    }

    private void Move_performed(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        Debug.Log($"[InputHandler] 🔵 Move: {direction}");
        OnMoveChanged?.Invoke(direction);
    }

    private void Move_canceled(InputAction.CallbackContext context)
    {
        Debug.Log("[InputHandler] ⛔ Move bị hủy");
        OnMoveChanged?.Invoke(Vector2.zero);
    }

    // (Giữ lại cho trường hợp cần)
    public Vector2 GetMovementInput()
    {
        if (!enabled || !inputActions.Player.enabled)
            return Vector2.zero;

        return inputActions.Player.Move.ReadValue<Vector2>();
    }
}
