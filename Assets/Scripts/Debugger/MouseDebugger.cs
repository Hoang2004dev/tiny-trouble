using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem; // <- QUAN TRỌNG

public class MouseDebugger : MonoBehaviour
{
    //void Update()
    //{
    //    // Dùng Input System mới để kiểm tra click chuột trái
    //    if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
    //    {
    //        Vector2 mousePos = Mouse.current.position.ReadValue();
    //        Debug.Log($"🖱️ Click detected at: {mousePos}");

    //        // Kiểm tra xem đang click vào UI
    //        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
    //        {
    //            Debug.Log("🛑 Click bị block bởi UI element");
    //        }
    //        else
    //        {
    //            // Raycast kiểm tra đối tượng
    //            Ray ray = Camera.main.ScreenPointToRay(mousePos);
    //            if (Physics.Raycast(ray, out RaycastHit hit))
    //            {
    //                Debug.Log($"🎯 Click hit 3D: {hit.collider.name}");
    //            }
    //            else
    //            {
    //                Debug.Log("🌌 Click không trúng đối tượng nào");
    //            }
    //        }
    //    }
    //}
}
