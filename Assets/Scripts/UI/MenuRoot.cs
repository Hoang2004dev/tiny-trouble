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
            Debug.Log("[MenuRoot] Singleton instance.");

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
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
        if (scene.name == "StartScene")
        {
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
    }

    private void UpdateCanvasCamera()
    {
        if (menuCanvas == null)
        {
            Debug.LogWarning("[MenuRoot] MenuCanvas chưa được gán trong inspector!");
            return;
        }

        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            menuCanvas.worldCamera = mainCam;
        }
        else
        {
            Debug.LogWarning("[MenuRoot] Không tìm thấy MainCamera trong scene!");
        }
    }

    private void UpdateCanvasSorting()
    {
        if (menuCanvas == null)
        {
            Debug.LogWarning("[MenuRoot] MenuCanvas chưa được gán trong inspector!");
            return;
        }

        menuCanvas.sortingLayerName = "Player";  
        menuCanvas.sortingOrder = 10;        
    }
}
