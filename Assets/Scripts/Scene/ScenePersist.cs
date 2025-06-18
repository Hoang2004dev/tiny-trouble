using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    public static ScenePersist Instance { get; private set; }
    public bool shouldPlayExitTransition = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
