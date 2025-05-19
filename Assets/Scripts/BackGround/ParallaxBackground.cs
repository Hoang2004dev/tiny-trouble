using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform target; // Thường là Main Camera hoặc Player
    [SerializeField] private float parallaxEffect; // Tốc độ di chuyển (0 = tĩnh, 1 = di chuyển cùng camera)
    [SerializeField] private float length; // Chiều dài sprite (tính bằng unit)
    private float startPosX;

    void Start()
    {
        startPosX = transform.position.x;
        // Nếu không Roslinh lấy chiều dài sprite từ SpriteRenderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            length = spriteRenderer.bounds.size.x;
        }
    }

    void LateUpdate()
    {
        // Tính khoảng cách camera di chuyển
        float distance = target.position.x * parallaxEffect;
        float temp = (target.position.x * (1 - parallaxEffect));

        // Cập nhật vị trí lớp nền
        float newPosX = startPosX + distance;

        // Cuộn vô hạn
        if (temp > startPosX + length)
            startPosX += length;
        else if (temp < startPosX - length)
            startPosX -= length;

        transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
    }
}