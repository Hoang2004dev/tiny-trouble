using UnityEngine;

public class NPCEmojiController : MonoBehaviour
{
    [Header("Emoji hiển thị")]
    public GameObject emojiObject; // Object chứa Sprite + Animator

    private Animator emojiAnimator;
    private bool isPlayerNear = false;

    [Header("Tên trigger để chạy animation (tùy emoji)")]
    public string animationTrigger = "Pop"; // Mỗi emoji dùng animation riêng

    void Start()
    {
        if (emojiObject != null)
        {
            emojiAnimator = emojiObject.GetComponent<Animator>();
            emojiObject.SetActive(false); // ẩn lúc đầu
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPlayerNear)
        {
            isPlayerNear = true;

            emojiObject.SetActive(true);
            if (emojiAnimator != null && !string.IsNullOrEmpty(animationTrigger))
            {
                emojiAnimator.SetTrigger(animationTrigger); // Cho phép dùng trigger riêng
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            emojiObject.SetActive(false);
        }
    }
}
