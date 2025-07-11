using UnityEngine;
using Unity.Cinemachine;

public class TransitionTrigger : MonoBehaviour
{
    void Start()
    {
        Debug.Log("[TransitionTrigger] TransitionTrigger.Start được gọi!");

        if (ScenePersist.Instance != null && ScenePersist.Instance.shouldPlayExitTransition)
        {
            ScenePersist.Instance.shouldPlayExitTransition = false;

            var tm = FindFirstObjectByType<TransitionManager>();
            if (tm != null)
            {
                tm.SpawnBlocksCoverScreen();
                tm.Play(TransitionType.Exit);
            }
            else
            {
                Debug.LogWarning("[TransitionTrigger] Không tìm thấy TransitionManager!");
            }
        }
        else
        {
            Debug.Log("[TransitionTrigger] ScenePersist.Instance == null hoặc shouldPlayExitTransition = false");
        }

        PlayerHealth.isTransitioning = false;

        // ⭐ Add this line
        UIUtils.SetAllButtonsInteractableAllScenes(true);
    }
}
