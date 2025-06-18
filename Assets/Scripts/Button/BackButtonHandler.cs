using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackButtonHandle : MonoBehaviour
{
    public static BackButtonHandle Instance { get; private set; }

    [Header("Scene config")]
    public string menuSceneName = "LevelSelectScene";
    public string homeSceneName = "StartScene";

    [Header("Button reference")]
    public Button backButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[BackButton] ❌ Đã tồn tại instance khác, hủy object này.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("[BackButton] ✅ Instance khởi tạo và giữ lại qua các scene.");

        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonPressed);
            Debug.Log("[BackButton] 🔗 Đã gán sự kiện OnClick cho backButton.");
        }
        else
        {
            Debug.LogWarning("[BackButton] ⚠️ Button chưa được gán trong Inspector.");
        }
    }

    private void OnBackButtonPressed()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        string targetScene = null;

        Debug.Log($"[BackButton] 🔘 Nút Back được nhấn tại scene: {currentScene}");

        if (currentScene.StartsWith("Lv"))
        {
            targetScene = menuSceneName;
            Debug.Log("[BackButton] ↩️ Từ Level → chuyển về Menu.");
        }
        else if (currentScene == menuSceneName)
        {
            targetScene = homeSceneName;
            Debug.Log("[BackButton] ↩️ Từ Menu → chuyển về Home.");
        }
        else if (currentScene == homeSceneName)
        {
            Debug.Log("[BackButton] 🔴 Từ Home → Thoát game.");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            return;
        }

        if (!string.IsNullOrEmpty(targetScene))
        {
            LoadSceneWithTransition(targetScene);
        }
        else
        {
            Debug.LogWarning("[BackButton] ⚠️ Không rõ hành vi tại scene: " + currentScene);
        }
    }

    private void LoadSceneWithTransition(string sceneName)
    {
        Debug.Log($"[BackButton] 📦 Chuẩn bị chuyển scene tới: {sceneName}");

        TransitionManager tm = FindFirstObjectByType<TransitionManager>();
        if (tm != null)
        {
            if (ScenePersist.Instance != null)
            {
                ScenePersist.Instance.shouldPlayExitTransition = true;
                Debug.Log("[BackButton] ✅ Đã bật cờ shouldPlayExitTransition trong ScenePersist.");
            }
            else
            {
                Debug.LogWarning("[BackButton] ⚠️ ScenePersist.Instance chưa tồn tại.");
            }

            Debug.Log("[BackButton] 🎬 Chạy hiệu ứng chuyển cảnh Enter.");
            tm.Play(TransitionType.Enter, () =>
            {
                Debug.Log($"[BackButton] 🎯 Gọi LoadScene: {sceneName}");
                SceneManager.LoadScene(sceneName);
            });
        }
        else
        {
            Debug.LogWarning("[BackButton] ⚠️ Không tìm thấy TransitionManager, chuyển scene trực tiếp.");
            SceneManager.LoadScene(sceneName);
        }
    }
}
