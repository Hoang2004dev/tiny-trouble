using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    [Header("Panels")]
    [Tooltip("Script animator quản lý mở/đóng MenuPanel")]
    public MenuPanelAnimator animator;

    [Tooltip("Overlay chứa background blocker + MusicSettingsPanel")]
    public GameObject musicOverlay;

    public void GoToStartScene()
    {
        LoadSceneIfNeeded("StartScene");
    }

    public void GoToLevelSelectScene()
    {
        LoadSceneIfNeeded("LevelSelectScene");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void OpenMenu()
    {
        if (animator != null)
        {
            animator.Open();
        }
    }

    public void CloseMenu()
    {
        if (animator != null)
        {
            animator.Close();
        }
    }

    public void OpenMusicSettings()
    {
        if (musicOverlay != null)
        {
            musicOverlay.SetActive(true);
        }
    }

    public void CloseMusicSettings()
    {
        if (musicOverlay != null)
        {
            musicOverlay.SetActive(false);
        }
    }

    public void OnBackgroundClick()
    {
        CloseMusicSettings();
    }

    private void LoadSceneIfNeeded(string sceneName)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == sceneName)
        {
            return;
        }

        Close();

        StartSceneTransition(sceneName);
    }

    private void Close()
    {
        CloseMenu();
        CloseMusicSettings();
    }

    private void StartSceneTransition(string sceneName)
    {
            SceneManager.LoadScene(sceneName);
    }

    public void ToggleMenu()
    {
        if (animator == null || animator.panel == null)
        {
            return;
        }

        if (animator.panel.activeSelf)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }
}
