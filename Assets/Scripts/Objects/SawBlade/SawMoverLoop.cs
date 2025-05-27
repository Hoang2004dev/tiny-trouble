using UnityEngine;

public class SawMoverLoop : MonoBehaviour
{
    public Transform[] waypoints; // Các điểm A → B → C → D
    public float speed = 2f;

    private int currentTargetIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentTargetIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            currentTargetIndex = (currentTargetIndex + 1) % waypoints.Length;
        }
    }
}
