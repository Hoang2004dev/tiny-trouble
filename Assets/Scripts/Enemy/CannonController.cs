using UnityEngine;

public class CannonController : MonoBehaviour
{
    [Header("Cannon Settings")]
    public GameObject cannonBallPrefab;
    public Transform firePoint;
    public Transform targetTransform;

    [Header("Auto Fire Settings")]
    public bool isAutoFire = false;
    public float autoFireInterval = 1f;

    [Header("Animation")]
    private Animator animator;

    private float fireTimer = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator component is missing on Cannon object!");
        }
    }

    void Update()
    {
        if (isAutoFire)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= autoFireInterval)
            {
                fireTimer = 0f;
                Shoot();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // trigger  animation
        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }

        // tạo đạn
        GameObject bullet = Instantiate(cannonBallPrefab, firePoint.position, Quaternion.identity);
        CannonBall cb = bullet.GetComponent<CannonBall>();
        if (cb != null)
        {
            cb.SetTarget(targetTransform.position);
        }
    }
}
