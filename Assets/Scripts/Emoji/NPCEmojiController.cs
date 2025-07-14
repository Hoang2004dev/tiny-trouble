using UnityEngine;

public class NPCEmojiController : MonoBehaviour
{
    [Header("Emoji hiển thị")]
    public GameObject emojiObject; 

    private Animator emojiAnimator;
    private bool isPlayerNear = false;

    [Header("Tên trigger để chạy animation (tùy emoji)")]
    public string animationTrigger = "Pop"; 

    void Start()
    {
        if (emojiObject != null)
        {
            emojiAnimator = emojiObject.GetComponent<Animator>();
            emojiObject.SetActive(false); 
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
                emojiAnimator.SetTrigger(animationTrigger); 
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
