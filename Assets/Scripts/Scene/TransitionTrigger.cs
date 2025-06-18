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

        // Bật lại PlayerInputHandler nếu đang bị vô hiệu hóa
        //if (PlayerInputHandler.Instance != null && !PlayerInputHandler.Instance.enabled)
        //{
        //    Debug.Log("[TransitionTrigger] Bật lại PlayerInputHandler sau scene load");
        //    PlayerInputHandler.Instance.enabled = true;
        //}
   
        PlayerHealth.isTransitioning = false;
    }
}
