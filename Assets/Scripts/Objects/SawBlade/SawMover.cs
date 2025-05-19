using UnityEngine;

public class SawMover : MonoBehaviour
{
    public Transform pointA; // điểm bắt đầu
    public Transform pointB; // điểm kết thúc
    public float speed = 2f;

    private Vector3 target;

    void Start()
    {
        target = pointB.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            target = target == pointA.position ? pointB.position : pointA.position;
        }
    }
}
