using UnityEngine;
using UnityEngine.UI;

public class SettingsButtonHandle : MonoBehaviour
{
    [Header("Button reference")]
    public Button settingsButton;

    void Awake()
    {
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OnSettingsButtonPressed);
            Debug.Log("[SettingsButton] Đã gán sự kiện OnClick cho settingsButton.");
        }
        else
        {
            Debug.LogWarning("[SettingsButton] Button chưa được gán trong Inspector!");
        }
    }

    private void OnSettingsButtonPressed()
    {
        var menuNav = FindFirstObjectByType<MenuNavigation>();
        if (menuNav != null)
        {
            menuNav.ToggleMenu();
        }
        else
        {
            Debug.LogError("[SettingsButton] Không tìm thấy MenuNavigation trong scene!");
        }
    }
}
