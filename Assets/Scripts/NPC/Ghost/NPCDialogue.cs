using UnityEngine;
using TMPro;
using System.Collections;

public class NPCDialogue : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    [TextArea(2, 5)] public string[] dialogueLines;

    [Header("Typing Settings")]
    public float typingSpeed = 0.03f;

    private int currentLine = 0;
    private bool playerInRange = false;
    private bool isDialogueActive = false;
    private bool isTyping = false;

    private Coroutine typingCoroutine;

    public AudioClip typingSound;
    private AudioSource audioSource;

    [Header("Speaker Info")]
    public string speakerName;
    public Sprite speakerAvatar;

    public TextMeshProUGUI speakerNameText;
    public UnityEngine.UI.Image avatarImage;

    [Header("Event After Dialogue")]
    public GameObject objectToActivateAfterDialogue;
    private bool hasActivatedEvent = false;

    void OnEnable()
    {
        if (PlayerInputHandler.Instance != null)
            PlayerInputHandler.Instance.OnInteractPressed += HandleInteraction;

        audioSource = GetComponent<AudioSource>();
    }

    void OnDisable()
    {
        if (PlayerInputHandler.Instance != null)
            PlayerInputHandler.Instance.OnInteractPressed -= HandleInteraction;
    }

    void HandleInteraction()
    {
        if (!playerInRange) return;

        if (!isDialogueActive)
        {
            StartDialogue();
        }
        else if (isTyping)
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

        if (speakerNameText != null)
            speakerNameText.text = speakerName;

        if (avatarImage != null)
            avatarImage.sprite = speakerAvatar;

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
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        dialogueUI.SetActive(false);
        isTyping = false;
        currentLine = 0;

        if (!hasActivatedEvent && objectToActivateAfterDialogue != null)
        {
            objectToActivateAfterDialogue.SetActive(true);
            hasActivatedEvent = true;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            EndDialogue();
        }
    }
}
