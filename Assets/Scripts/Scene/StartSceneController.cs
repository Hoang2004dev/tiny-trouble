using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class StartSceneController : MonoBehaviour
{
    public TextMeshProUGUI touchToPlayText;
    private PlayerInputActions inputActions;

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
        SceneManager.LoadScene("LevelSelectScene"); // đổi tên phù hợp

        // phải thêm scene vào danh sách build trong Unity mới load dc
    }
}
