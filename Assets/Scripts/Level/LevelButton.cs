using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [Header("Tên Scene Level cần load")]
    public string levelName;

    [Header("Text hiển thị khi bị khóa")]
    public string lockedText = "???";

    [Header("Text sẽ bị đổi khi bị khóa (kéo vào đây trong Inspector)")]
    public TextMeshProUGUI targetText;

    private Button button;
    private string originalText;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (targetText != null)
        {
            originalText = targetText.text;
        }
        else
        {
            Debug.LogWarning($"[LevelButton] Chưa gán `targetText` cho button {gameObject.name}!");
        }
    }

    private void Start()
    {
        bool unlocked = LevelProgress.IsLevelUnlocked(levelName);
        button.interactable = unlocked;

        if (targetText != null)
        {
            targetText.text = unlocked ? originalText : lockedText;

            Debug.Log($"[LevelButton] Nút {levelName} {(unlocked ? "MỞ" : "KHÓA")}: hiển thị \"{targetText.text}\"");
        }
    }
}
