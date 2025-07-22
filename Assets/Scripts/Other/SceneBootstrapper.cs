using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-999)]
public class SceneBootstrapper : MonoBehaviour
{
    [SerializeField] private string startupSceneName = "MainMenu";
    private static bool hasBootstrapped = false;

    void Awake()
    {
        if (hasBootstrapped) return; 

        if (SceneManager.GetActiveScene().name != startupSceneName)
        {
            hasBootstrapped = true;
            SceneManager.LoadScene(startupSceneName);
        }
    }
}
