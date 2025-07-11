using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuRoot : MonoBehaviour
{
    public static MenuRoot Instance { get; private set; }

    [Header("Canvas tham chiếu")]
    [Tooltip("Canvas của MenuRoot (phải là Screen Space - Camera)")]
    [SerializeField] private Canvas menuCanvas;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[MenuRoot] ✅ Singleton instance giữ lại qua scene.");

            // Đăng ký lắng nghe khi scene mới load xong
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Debug.LogWarning("[MenuRoot] ⚠️ Duplicate MenuRoot bị huỷ.");
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[MenuRoot] ✅ Scene loaded: {scene.name}. Xử lý bật/tắt, nút, camera, sorting.");

        if (scene.name == "StartScene")
        {
            Debug.Log("[MenuRoot] 📴 Ẩn MenuRoot trong StartScene.");
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }

        EnableAllButtons();
        UpdateCanvasCamera();
        UpdateCanvasSorting();
    }

    private void EnableAllButtons()
    {
        Button[] buttons = GetComponentsInChildren<Button>(true);
        foreach (var btn in buttons)
        {
            btn.interactable = true;
        }

        Debug.Log("[MenuRoot] ✅ Tất cả nút trong MenuRoot đã bật lại interactable.");
    }

    private void UpdateCanvasCamera()
    {
        if (menuCanvas == null)
        {
            Debug.LogWarning("[MenuRoot] ⚠️ MenuCanvas chưa được gán trong inspector!");
            return;
        }

        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            menuCanvas.worldCamera = mainCam;
            Debug.Log($"[MenuRoot] ✅ Canvas Camera đã set thành MainCamera của scene: {mainCam.name}");
        }
        else
        {
            Debug.LogWarning("[MenuRoot] ⚠️ Không tìm thấy MainCamera trong scene!");
        }
    }

    private void UpdateCanvasSorting()
    {
        if (menuCanvas == null)
        {
            Debug.LogWarning("[MenuRoot] ⚠️ MenuCanvas chưa được gán trong inspector!");
            return;
        }

        menuCanvas.sortingLayerName = "Player";  // Hoặc tên layer bạn muốn
        menuCanvas.sortingOrder = 10;        // Hoặc thứ tự bạn muốn

        Debug.Log($"[MenuRoot] ✅ Đã set SortingLayer = {menuCanvas.sortingLayerName}, Order = {menuCanvas.sortingOrder}");
    }
}
