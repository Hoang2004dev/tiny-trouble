using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Điều khiển MenuPanel chính (nút Home, Level, Music, Quit, Close)
/// và xử lý chuyển scene với transition.
/// </summary>
public class MenuNavigation : MonoBehaviour
{
    [Header("Panels")]
    [Tooltip("Script animator quản lý mở/đóng MenuPanel")]
    public MenuPanelAnimator animator;

    [Tooltip("Overlay chứa background blocker + MusicSettingsPanel")]
    public GameObject musicOverlay;

    /// <summary>
    /// Chuyển về StartScene với transition.
    /// </summary>
    public void GoToStartScene()
    {
        LoadSceneIfNeeded("StartScene");
    }

    /// <summary>
    /// Chuyển về LevelSelectScene với transition.
    /// </summary>
    public void GoToLevelSelectScene()
    {
        LoadSceneIfNeeded("LevelSelectScene");
    }

    /// <summary>
    /// Thoát game (hoặc dừng play mode trong Editor).
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("[MenuNavigation] 🔴 QuitGame() called!");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Hiện MenuPanel với animation.
    /// </summary>
    public void OpenMenu()
    {
        if (animator != null)
        {
            animator.Open();
        }
    }

    /// <summary>
    /// Đóng MenuPanel với animation.
    /// </summary>
    public void CloseMenu()
    {
        if (animator != null)
        {
            animator.Close();
        }
    }

    /// <summary>
    /// Hiện MusicSettingsPanel overlay.
    /// </summary>
    public void OpenMusicSettings()
    {
        if (musicOverlay != null)
        {
            musicOverlay.SetActive(true);
        }
    }

    /// <summary>
    /// Đóng MusicSettingsPanel overlay.
    /// </summary>
    public void CloseMusicSettings()
    {
        if (musicOverlay != null)
        {
            musicOverlay.SetActive(false);
        }
    }

    /// <summary>
    /// Được gọi từ BackgroundBlocker Button → đóng MusicSettingsPanel.
    /// </summary>
    public void OnBackgroundClick()
    {
        CloseMusicSettings();
    }

    /// <summary>
    /// Kiểm tra và load scene nếu cần, với transition animation.
    /// </summary>
    /// <param name="sceneName">Tên scene muốn chuyển tới.</param>
    private void LoadSceneIfNeeded(string sceneName)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == sceneName)
        {
            Debug.Log($"[MenuNavigation] ⚠️ Scene hiện tại đã là '{sceneName}', không load lại.");
            return;
        }

        Debug.Log($"[MenuNavigation] 📦 Chuẩn bị chuyển scene từ '{currentScene}' tới: '{sceneName}'");

        // Đóng MenuPanel + MusicOverlay ngay lập tức
        Close();

        // Thực hiện hiệu ứng chuyển cảnh và load scene
        StartSceneTransition(sceneName);
    }

    /// <summary>
    /// Đóng tất cả UI Menu ngay lập tức.
    /// </summary>
    private void Close()
    {
        CloseMenu();
        CloseMusicSettings();
    }

    /// <summary>
    /// Bắt đầu transition animation và load scene.
    /// </summary>
    /// <param name="sceneName">Tên scene muốn load.</param>
    private void StartSceneTransition(string sceneName)
    {
        TransitionManager tm = FindFirstObjectByType<TransitionManager>();

        if (tm != null)
        {
            // Đặt flag cho TransitionTrigger trong scene mới
            if (ScenePersist.Instance != null)
            {
                ScenePersist.Instance.shouldPlayExitTransition = true;
                Debug.Log("[MenuNavigation] ✅ Đã bật cờ shouldPlayExitTransition trong ScenePersist.");
            }

            Debug.Log("[MenuNavigation] 🎬 Chạy hiệu ứng Transition.Enter");
            tm.Play(TransitionType.Enter, () =>
            {
                Debug.Log($"[MenuNavigation] 🎯 Thực sự gọi LoadScene: {sceneName}");
                SceneManager.LoadScene(sceneName);
            });
        }
        else
        {
            Debug.LogWarning("[MenuNavigation] ⚠️ Không tìm thấy TransitionManager → LoadScene trực tiếp.");
            SceneManager.LoadScene(sceneName);
        }
    }

    public void ToggleMenu()
    {
        if (animator == null || animator.panel == null)
        {
            Debug.LogWarning("[MenuNavigation] ⚠️ Animator hoặc Panel chưa được gán!");
            return;
        }

        if (animator.panel.activeSelf)
        {
            Debug.Log("[MenuNavigation] 🔻 Panel đang mở → đóng lại.");
            CloseMenu();
        }
        else
        {
            Debug.Log("[MenuNavigation] 🔺 Panel đang tắt → mở ra.");
            OpenMenu();
        }
    }
}
