using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [Header("Texture của con trỏ chuột")]
    public Texture2D cursorTexture;

    [Header("Vị trí 'đầu nhọn' của con trỏ")]
    public Vector2 hotspot = Vector2.zero;

    [Header("Kiểu con trỏ")]
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        if (cursorTexture != null)
        {
            Cursor.SetCursor(cursorTexture, hotspot, cursorMode);   
        }
    }
}
