using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SimpleNPCDialogue : MonoBehaviour
{
    public GameObject player;
    public PLAYERCONTROL2 playerMovementScript;
    public GameObject interactPrompt;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public Image iconImage;
    public Button nextButton;
    public Animator dialogueAnimator;

    public NPCMovement npcMovement;
    public string[] dialogueLines;
    public Sprite npcIcon;
    public string npcName = "NPC";

    public float typewriterSpeed = 0.05f;

    public GameObject[] encyclopediaObjects;
    public GameObject[] objectstoturnoff;
    public TextMeshProUGUI messageText; // Message to display
    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;
    public float messageDisplayTime = 2f;

    public string[] fadeInMessages; // Messages to fade in

    private bool isPlayerNearby = false;
    private bool isTalking = false;
    private int currentDialogueIndex = 0;
    private Coroutine currentTypewriterCoroutine;

    void Start()
    {
        ValidateInspectorReferences();
        interactPrompt.SetActive(false);
        dialoguePanel.SetActive(false);
        nextButton.onClick.AddListener(ShowNextDialogue);
    }

    void Update()
    {
        if (isPlayerNearby && !isTalking && Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue(dialogueLines, npcIcon, npcName);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTalking) // Prevents dialogue from closing mid-interaction
        {
            isPlayerNearby = false;
            interactPrompt.SetActive(false);
        }
    }

    private void StartDialogue(string[] dialogue, Sprite speakerIcon, string speakerName)
    {
        if (dialogue.Length == 0) return;

        if (playerMovementScript)
        {
            playerMovementScript.canMove = false; // ✅ Disable player movement
            player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // ✅ Stop movement instantly
        }
        if (npcMovement) npcMovement.enabled = false;

        isTalking = true;
        dialoguePanel.SetActive(true);
        dialogueAnimator.SetBool("IsOpen", true);
        currentDialogueIndex = 0;
        ShowDialogueLine(dialogue, speakerIcon, speakerName);
    }

    private void ShowDialogueLine(string[] dialogue, Sprite speakerIcon, string speakerName)
    {
        if (currentTypewriterCoroutine != null)
        {
            StopCoroutine(currentTypewriterCoroutine);
            currentTypewriterCoroutine = null;
        }

        if (currentDialogueIndex < dialogue.Length)
        {
            dialogueText.text = "";
            nameText.text = speakerName;
            iconImage.sprite = speakerIcon;

            currentTypewriterCoroutine = StartCoroutine(TypewriterEffect(dialogue[currentDialogueIndex]));
            currentDialogueIndex++;
        }
        else
        {
            EndDialogue();
        }
    }

    private IEnumerator TypewriterEffect(string dialogueLine)
    {
        dialogueText.text = "";
        foreach (char letter in dialogueLine)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typewriterSpeed);
        }
        currentTypewriterCoroutine = null;
    }

    public void ShowNextDialogue()
    {
        if (isTalking)
        {
            if (currentTypewriterCoroutine != null)
            {
                StopCoroutine(currentTypewriterCoroutine);
                dialogueText.text = dialogueLines[currentDialogueIndex - 1];
                currentTypewriterCoroutine = null;
            }
            else if (currentDialogueIndex < dialogueLines.Length)
            {
                ShowDialogueLine(dialogueLines, npcIcon, npcName);
            }
            else
            {
                EndDialogue();
            }
        }
    }

    private void EndDialogue()
    {
        dialogueAnimator.SetBool("IsOpen", false);
        StartCoroutine(WaitForClosingAnimation());
        ActivateEncyclopedia();
        turnoffobjects();
    }
    private void turnoffobjects()
    {
        // Activate multiple GameObjects
        foreach (GameObject obj in objectstoturnoff)
        {
            obj.SetActive(false);
        }

    }
    private void ActivateEncyclopedia()
    {
        // Activate multiple GameObjects
        foreach (GameObject obj in encyclopediaObjects)
        {
            obj.SetActive(true);
        }

        // Start fading in the messages sequentially
        StartCoroutine(DisplayMessagesSequentially(fadeInMessages));
    }

    private IEnumerator DisplayMessagesSequentially(string[] messages)
    {
        foreach (string message in messages)
        {
            yield return StartCoroutine(FadeInMessage(message));
        }
    }

    private IEnumerator FadeInMessage(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);

        float elapsedTime = 0f;
        Color initialColor = messageText.color;
        initialColor.a = 0f;
        messageText.color = initialColor;

        while (elapsedTime < fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            initialColor.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeInTime);
            messageText.color = initialColor;
            yield return null;
        }

        yield return new WaitForSeconds(messageDisplayTime);

        elapsedTime = 0f;
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            initialColor.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutTime);
            messageText.color = initialColor;
            yield return null;
        }

        messageText.gameObject.SetActive(false);
    }

    private IEnumerator WaitForClosingAnimation()
    {
        yield return new WaitForSeconds(2f);

        dialoguePanel.SetActive(false);
        isTalking = false;

        if (playerMovementScript)
        {
            playerMovementScript.canMove = true; // ✅ Re-enable movement
        }

        if (npcMovement) npcMovement.enabled = true;
    }

    private void ValidateInspectorReferences()
    {
        if (!interactPrompt || !dialoguePanel || !dialogueText || !nameText || !iconImage || !nextButton || !dialogueAnimator)
        {
            Debug.LogError("One or more references are missing in the Inspector!");
        }
        if (!player || !playerMovementScript)
        {
            Debug.LogError("Player or PlayerMovementScript reference is missing in the Inspector!");
        }
    }
}
