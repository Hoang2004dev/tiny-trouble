using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class StartSceneController : MonoBehaviour
{
    public TextMeshProUGUI touchToPlayText;
    private PlayerInputActions inputActions;
    private bool loadingStarted = false;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Interact.performed += OnInteractPerformed;
    }

    void OnDisable()
    {
        inputActions.Player.Interact.performed -= OnInteractPerformed;
        inputActions.Disable();
    }

    void Start()
    {
        StartCoroutine(BlinkText());
    }

    void Update()
    {
        if (loadingStarted) return;

        if ((Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            || (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame))
        {
            LoadNextScene();
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        LoadNextScene();
    }

    IEnumerator BlinkText()
    {
        while (true)
        {
            touchToPlayText.alpha = 1;
            yield return new WaitForSeconds(0.5f);
            touchToPlayText.alpha = 0;
            yield return new WaitForSeconds(0.5f);
        }
    }

    void LoadNextScene()
    {
        if (loadingStarted) return;

        loadingStarted = true;
        SceneManager.LoadScene("LevelSelectScene");
    }
}
