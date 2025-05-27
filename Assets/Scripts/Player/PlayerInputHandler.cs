using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerInputHandler Instance { get; private set; }

    private PlayerInputActions inputActions;

    // Các sự kiện để các object khác có thể đăng ký lắng nghe
    public delegate void InteractAction();
    public event InteractAction OnInteractPressed;

    public delegate void JumpAction();
    public event JumpAction OnJumpPressed;

    private void Awake()
    {
        // Singleton an toàn, tránh trùng khi đổi scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Giữ lại qua các scene

        inputActions = new PlayerInputActions(); // Instantiate input
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        SubscribeInputEvents();
    }

    private void OnDisable()
    {
        if (Instance == this && inputActions != null)
        {
            UnsubscribeInputEvents();
            inputActions.Player.Disable();
        }
    }


    private void SubscribeInputEvents()
    {
        inputActions.Player.Interact.performed += Interact_performed;
        inputActions.Player.Jump.performed += Jump_performed;
    }

    private void UnsubscribeInputEvents()
    {
        inputActions.Player.Interact.performed -= Interact_performed;
        inputActions.Player.Jump.performed -= Jump_performed;
    }

    private void Interact_performed(InputAction.CallbackContext context)
    {
        OnInteractPressed?.Invoke();
    }

    private void Jump_performed(InputAction.CallbackContext context)
    {
        OnJumpPressed?.Invoke();
    }

    // Expose giá trị movement (Vector2) để character controller có thể gọi
    public Vector2 GetMovementInput()
    {
        return inputActions.Player.Move.ReadValue<Vector2>();
    }
}
