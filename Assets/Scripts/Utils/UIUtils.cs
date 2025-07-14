using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class UIUtils
{
    /// <summary>
    /// Bật/tắt tất cả Button trong 1 scene cụ thể (chỉ nếu scene đó đang active).
    /// </summary>
    /// <param name="sceneName">Tên scene cần áp dụng</param>
    /// <param name="isInteractable">True: bật; False: tắt</param>
    public static void SetAllButtonsInteractableInScene(string sceneName, bool isInteractable)
    {
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            return;
        }

        Button[] buttons = Object.FindObjectsByType<Button>(FindObjectsSortMode.None);

        foreach (Button btn in buttons)
        {
            btn.interactable = isInteractable;
        }
    }

    /// <summary>
    /// Bật/tắt tất cả Button trong tất cả scene hiện đang loaded.
    /// </summary>
    /// <param name="isInteractable">True: bật; False: tắt</param>
    public static void SetAllButtonsInteractableAllScenes(bool isInteractable)
    {
        Button[] buttons = Object.FindObjectsByType<Button>(FindObjectsSortMode.None);

        foreach (Button btn in buttons)
        {
            btn.interactable = isInteractable;
        }
    }
}
