using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingDialogue : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    [TextArea(2, 5)] public string[] dialogueLines;

    [Header("Typing Settings")]
    public float typingSpeed = 0.03f;
    private int currentLine = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    public AudioClip typingSound;
    private AudioSource audioSource;

    [Header("Fade & Transition")]
    public CanvasGroup fadePanel;
    public float fadeDuration = 2f;
    [Tooltip("Tên Scene sẽ load sau khi kết thúc")]
    public string sceneToLoad = "StartScene";

    void OnEnable()
    {
        if (PlayerInputHandler.Instance != null)
        {
            PlayerInputHandler.Instance.OnInteractPressed += HandleInteraction;
        }

        audioSource = GetComponent<AudioSource>();
        StartDialogue(); // Auto-start ngay khi scene load
    }

    void OnDisable()
    {
        if (PlayerInputHandler.Instance != null)
        {
            PlayerInputHandler.Instance.OnInteractPressed -= HandleInteraction;
        }
    }

    private void HandleInteraction()
    {
        if (!isDialogueActive) return;

        if (isTyping)
        {
            SkipTyping();
        }
        else
        {
            NextLine();
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        currentLine = 0;
        dialogueUI.SetActive(true);

        StartTyping(dialogueLines[currentLine]);
    }

    void NextLine()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            StartTyping(dialogueLines[currentLine]);
        }
        else
        {
            StartCoroutine(EndDialogueAndTransition());
        }
    }

    IEnumerator EndDialogueAndTransition()
    {
        isDialogueActive = false;
        isTyping = false;
        dialogueUI.SetActive(false);

        // Fade Out
        if (fadePanel != null)
        {
            float t = 0;
            while (t < fadeDuration)
            {
                fadePanel.alpha = t / fadeDuration;
                t += Time.deltaTime;
                yield return null;
            }
            fadePanel.alpha = 1;
        }

        // Load next scene
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    void StartTyping(string line)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        if (typingSound != null && audioSource != null)
        {
            audioSource.clip = typingSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }

        isTyping = false;
    }

    void SkipTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.text = dialogueLines[currentLine];
        isTyping = false;

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }
    }
}
