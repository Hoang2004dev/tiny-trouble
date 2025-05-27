using UnityEngine;

public class FloatingMotion : MonoBehaviour
{
    public float amplitude = 20f;       // Độ cao lên xuống (đơn vị pixel)
    public float frequency = 1f;        // Tốc độ nhịp chuyển động

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = startPos + new Vector3(0, offsetY, 0);
    }
}
