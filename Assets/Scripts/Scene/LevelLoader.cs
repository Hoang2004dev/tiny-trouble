using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadLevel(string levelName)
    {
        Debug.Log("[LevelLoader] Chuẩn bị chuyển scene: " + levelName);

        TransitionManager tm = FindFirstObjectByType<TransitionManager>();
        if (tm != null)
        {
            if (ScenePersist.Instance != null)
            {
                ScenePersist.Instance.shouldPlayExitTransition = true;
                Debug.Log("[LevelLoader] Đã bật cờ shouldPlayExitTransition");
            }
            else
            {
                Debug.LogWarning("[LevelLoader] ⚠️ ScenePersist.Instance chưa tồn tại");
            }

            Debug.Log("[LevelLoader] Tìm thấy TransitionManager, chạy hiệu ứng Enter");
            tm.Play(TransitionType.Enter, () =>
            {
                Debug.Log("[LevelLoader] Gọi LoadScene: " + levelName);
                SceneManager.LoadScene(levelName);
            });
        }
        else
        {
            Debug.LogWarning("[LevelLoader] Không tìm thấy TransitionManager, load scene trực tiếp");
            SceneManager.LoadScene(levelName);
        }
    }
}
