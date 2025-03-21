using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class npcquestgiver : MonoBehaviour
{
    public NPCFollower player;
    public QuestManager questManager;
    public GameObject interactPrompt;
    public GameObject dialoguePanel;
    public GameObject nutCollectorPanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public Image iconImage;
    public Button nextButton;
    public Animator dialogueAnimator;

    public string[] dialogueStartQuest;
    public string[] dialogueDuringQuest;
    public string[] dialogueQuestComplete;

    public Sprite squirrelIcon;
    public Sprite playerIcon;

    public float typewriterSpeed = 0.05f;

    private bool isPlayerNearby = false;
    private bool isQuestStarted = false;
    private bool isQuestCompleted = false;
    private bool isTalking = false;
    private int currentDialogueIndex = 0;

    private Coroutine currentTypewriterCoroutine;

    // New fields for Encyclopedia and TextMeshPro message
    public Button encyclopediaButton; // Reference to the Encyclopedia button
    public TextMeshProUGUI messageText; // TextMeshPro message to show after quest completion
    public float fadeInTime = 1f; // Time for the text to fade in
    public float fadeOutTime = 1f; // Time for the text to fade out
    public float messageDisplayTime = 2f; // How long the message stays visible



    // Array for GameObjects to enable
    [Header("GameObjects to Enable")]
    [SerializeField]
    private GameObject[] objectsToEnable;

    // Array for GameObjects to disable
    [Header("GameObjects to Disable")]
    [SerializeField]
    private GameObject[] objectsToDisable;

    [Header("NPC Movement")]
    [SerializeField] private Transform[] waypoints; // Waypoints for the NPC to move to
    [SerializeField] private float moveSpeed = 2f;  // Movement speed of the NPC
    private int currentWaypointIndex = 0;  // To track which waypoint the NPC is at
    private bool isMoving = false;  // To track if the NPC is moving
    private bool hasReachedFinalWaypoint = false;


    void Start()
    {
        ValidateInspectorReferences();
        interactPrompt.SetActive(false);
        dialoguePanel.SetActive(false);
        nutCollectorPanel.SetActive(false);
        encyclopediaButton.gameObject.SetActive(false); // Initially hide the encyclopedia button
        messageText.gameObject.SetActive(false); // Initially hide the message
        nextButton.onClick.AddListener(ShowNextDialogue);
    }

    void Update()
    {
        if (isPlayerNearby && !isTalking && !isMoving && Input.GetKeyDown(KeyCode.E))
        {
            if (!isQuestStarted)
            {
                StartDialogue(dialogueStartQuest, squirrelIcon, "Bunny");
            }
            else if (isQuestStarted && !isQuestCompleted && NPCFollower.followingCount < 4)
            {
                StartDialogue(dialogueDuringQuest, squirrelIcon, "Bunny");
            }
            else if (isQuestStarted && NPCFollower.followingCount >= 4)
            {
                ReturnNutsToSquirrel();
            }
            else if (isQuestCompleted)
            {
                StartDialogue(dialogueQuestComplete, squirrelIcon, "Bunny");
            }
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
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            interactPrompt.SetActive(false);
            dialoguePanel.SetActive(false);
            isTalking = false;
            currentDialogueIndex = 0;

            if (currentTypewriterCoroutine != null)
            {
                StopCoroutine(currentTypewriterCoroutine);
                currentTypewriterCoroutine = null;
            }
        }
    }

    private void StartDialogue(string[] dialogue, Sprite speakerIcon, string speakerName)
    {
        if (dialogue.Length == 0) return;
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
            string[] activeDialogue = GetActiveDialogue();

            if (currentTypewriterCoroutine != null)
            {
                StopCoroutine(currentTypewriterCoroutine);
                dialogueText.text = activeDialogue[currentDialogueIndex - 1]; // Show full text of the current line
                currentTypewriterCoroutine = null;
            }
            else if (currentDialogueIndex < activeDialogue.Length)
            {
                ShowDialogueLine(activeDialogue, squirrelIcon, "Bunny");
            }
            else
            {
                EndDialogue();
            }
        }
    }

    private string[] GetActiveDialogue()
    {
        if (!isQuestStarted)
            return dialogueStartQuest;
        if (!isQuestCompleted && NPCFollower.followingCount < 4)
            return dialogueDuringQuest;
        return dialogueQuestComplete;
    }

    private void EndDialogue()
    {
        dialogueAnimator.SetBool("IsOpen", false);
        StartCoroutine(WaitForClosingAnimation());
    }

    private IEnumerator WaitForClosingAnimation()
    {
        yield return new WaitForSeconds(2f);
        dialoguePanel.SetActive(false);
        isTalking = false;

        // After dialogue ends, start the NPC's movement only if it hasn't reached the final waypoint
        if (!hasReachedFinalWaypoint)
        {
            isMoving = true;
            StartCoroutine(MoveNPC());  // Start moving the NPC
        }

        if (!isQuestStarted)
        {
            StartFetchQuest();
        }
        else if (NPCFollower.followingCount >= 4 && !isQuestCompleted)
        {
            ReturnNutsToSquirrel();
        }
    }


    private void StartFetchQuest()
    {
        Debug.Log("Starting the Fetch Nuts quest!");
        questManager.StartBunnyQuest();
        isQuestStarted = true;
        nutCollectorPanel.SetActive(true);
    }

    private void ReturnNutsToSquirrel()
    {
        if (NPCFollower.followingCount >= 4)
        {
            Debug.Log("Returning 5 nuts to the squirrel!");
            NPCFollower.followingCount = 0;
            isQuestCompleted = true;
            questManager.CompleteBunnyQuest();
            nutCollectorPanel.SetActive(false);
            StartDialogue(dialogueQuestComplete, squirrelIcon, "Bunny");

            // After quest completion, activate the encyclopedia button and show the message
            ActivateEncyclopedia();
            EnableGameObjects();
            DisableGameObjects();
        }
        else
        {
            Debug.Log("Not enough nuts to return yet.");
        }
    }

    private void ActivateEncyclopedia()
    {
        encyclopediaButton.gameObject.SetActive(true); // Activate the Encyclopedia button
        StartCoroutine(FadeInMessage("QUEST COMPLETE: Bunny has been added to ENCYCLOPEDIA"));
    }

    private IEnumerator FadeInMessage(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);

        // Fade in the message
        float elapsedTime = 0f;
        Color initialColor = messageText.color;
        initialColor.a = 0f; // Start fully transparent
        messageText.color = initialColor;

        while (elapsedTime < fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            initialColor.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeInTime);
            messageText.color = initialColor;
            yield return null;
        }

        // Keep the message visible for a while
        yield return new WaitForSeconds(messageDisplayTime);

        // Fade out the message
        elapsedTime = 0f;
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            initialColor.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutTime);
            messageText.color = initialColor;
            yield return null;
        }

        messageText.gameObject.SetActive(false); // Hide the message after fade-out
    }

    private void ValidateInspectorReferences()
    {
        if (!player || !questManager || !interactPrompt || !dialoguePanel || !dialogueText || !nameText || !iconImage || !nextButton || !dialogueAnimator || !nutCollectorPanel || !encyclopediaButton || !messageText)
        {
            Debug.LogError("One or more references are missing in the Inspector!");
        }
    }

    public void ResetQuest()
    {
        isQuestStarted = false;
        isQuestCompleted = false;
        currentDialogueIndex = 0;
        nutCollectorPanel.SetActive(false);
        encyclopediaButton.gameObject.SetActive(false); // Hide the button after reset
        messageText.gameObject.SetActive(false); // Hide the message after reset
    }

    public void EnableGameObjects()
    {
        foreach (GameObject obj in objectsToEnable)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }

    public void DisableGameObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
    private IEnumerator MoveNPC()
    {
        while (currentWaypointIndex < waypoints.Length)
        {
            // Get the current waypoint position
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            while (Vector3.Distance(transform.position, targetWaypoint.position) > 0.1f)
            {
                // Move towards the waypoint
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);
                yield return null; // Wait until the next frame
            }

            // NPC reaches the waypoint
            currentWaypointIndex++;
        }

        // Once all waypoints are visited, stop moving
        isMoving = false;
        hasReachedFinalWaypoint = true; // Set the flag to true when the NPC reaches the final waypoint
    }

}