using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float parallaxEffect; // Tốc độ di chuyển cùng camera
    [SerializeField] private float length;
    private float startPosX;

    void Start()
    {
        startPosX = transform.position.x;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            length = spriteRenderer.bounds.size.x;
        }
    }

    void LateUpdate()
    {
        float distance = target.position.x * parallaxEffect;
        float temp = (target.position.x * (1 - parallaxEffect));

        float newPosX = startPosX + distance;

        if (temp > startPosX + length)
            startPosX += length;
        else if (temp < startPosX - length)
            startPosX -= length;

        transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
    }
}