using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [Header("Tên scene cần mở khóa")]
    public string levelToUnlock;

    [Header("Tên scene sẽ load")]
    public string levelToLoad;

    private bool isPlayerInRange = false;
    private bool hasActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            if (PlayerInputHandler.Instance != null)
            {
                PlayerInputHandler.Instance.OnInteractPressed += HandleInteract;
                Debug.Log("[Portal] Đăng ký sự kiện OnInteractPressed thành công");
            }
            else
            {
                Debug.LogWarning("[Portal] PlayerInputHandler.Instance chưa sẵn sàng!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("[Portal] Player rời khỏi vùng portal");

            if (PlayerInputHandler.Instance != null)
            {
                PlayerInputHandler.Instance.OnInteractPressed -= HandleInteract;
                Debug.Log("[Portal] Gỡ đăng ký sự kiện OnInteractPressed");
            }
        }
    }

    private void OnDestroy()
    {
        if (PlayerInputHandler.Instance != null)
        {
            PlayerInputHandler.Instance.OnInteractPressed -= HandleInteract;
            Debug.Log("[Portal] Gỡ đăng ký OnDestroy");
        }
    }

    private void HandleInteract()
    {
        Debug.Log("[Portal] HandleInteract được gọi");

        if (!isPlayerInRange)
        {
            Debug.LogWarning("[Portal] HandleInteract bị hủy: player không còn trong vùng");
            return;
        }

        if (hasActivated)
        {
            Debug.LogWarning("[Portal] Đã từng activate trước đó, không thực hiện lại");
            return;
        }

        hasActivated = true;

        PlayerInputHandler.Instance.OnInteractPressed -= HandleInteract;
        Debug.Log("[Portal] Bắt đầu hiệu ứng hút vào portal, chuẩn bị load scene: " + levelToLoad);

        StartCoroutine(AnimateAndEnterPortal());
    }

    private IEnumerator AnimateAndEnterPortal()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("[Portal] Không tìm thấy Player!");
            yield break;
        }

        PlayerHealth.isTransitioning = true;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        bool hadFreezeRotation = false;
        if (rb != null)
        {
            hadFreezeRotation = rb.freezeRotation;
            rb.freezeRotation = false;
            rb.angularVelocity = 0f;
        }

        Transform playerTransform = player.transform;
        SpriteRenderer sprite = player.GetComponent<SpriteRenderer>();

        float duration = 1.2f;
        float elapsed = 0f;
        Vector3 startPos = playerTransform.position;
        Vector3 targetPos = transform.position;
        Vector3 startScale = playerTransform.localScale;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            playerTransform.position = Vector3.Lerp(startPos, targetPos, t);
            playerTransform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            float rotationSpeed = Mathf.Lerp(180f, 720f, t);
            playerTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

            if (sprite != null)
            {
                Color c = sprite.color;
                c.a = Mathf.Lerp(1f, 0f, t);
                sprite.color = c;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        playerTransform.localScale = Vector3.zero;
        if (sprite != null)
        {
            Color c = sprite.color;
            c.a = 0f;
            sprite.color = c;
        }

        if (rb != null && hadFreezeRotation)
        {
            rb.freezeRotation = true;
            rb.angularVelocity = 0f;
            playerTransform.rotation = Quaternion.identity;
        }

        LevelProgress.UnlockLevel(levelToUnlock);

        SceneManager.LoadScene(levelToLoad);
    }
}
