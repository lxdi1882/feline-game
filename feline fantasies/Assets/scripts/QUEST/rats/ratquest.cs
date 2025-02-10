using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class RatQuest : MonoBehaviour
{
    public PlayerNutCollection playerNut;
    public QuestManager questManager;
    public GameObject interactPrompt;
    public GameObject dialoguePanel;
    public GameObject ratCollectorPanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public Image iconImage;
    public Button nextButton;
    public Animator dialogueAnimator;

    public string[] dialogueStartQuest;
    public string[] dialogueDuringQuest;
    public string[] dialogueQuestComplete;
    public Sprite ratIcon;

    public Button encyclopediaButton;
    public TextMeshProUGUI messageText;
    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;
    public float messageDisplayTime = 2f;

    private bool isPlayerNearby = false;
    private bool isQuestStarted = false;
    private bool isQuestCompleted = false;
    private bool isTalking = false;
    private int currentDialogueIndex = 0;

    private Coroutine currentTypewriterCoroutine;

    [Header("GameObjects to Enable")]
    [SerializeField] private GameObject[] objectsToEnable;

    [Header("GameObjects to Disable")]
    [SerializeField] private GameObject[] objectsToDisable;

    void Start()
    {
        ValidateInspectorReferences();
        interactPrompt.SetActive(false);
        dialoguePanel.SetActive(false);
        ratCollectorPanel.SetActive(false);
        encyclopediaButton.gameObject.SetActive(false);
        messageText.gameObject.SetActive(false);
        nextButton.onClick.AddListener(ShowNextDialogue);
    }

    void Update()
    {
        if (isPlayerNearby && !isTalking && Input.GetKeyDown(KeyCode.E))
        {
            if (!isQuestStarted)
            {
                StartDialogue(dialogueStartQuest);
            }
            else if (isQuestStarted && !isQuestCompleted && playerNut.ratkilled < 5)
            {
                StartDialogue(dialogueDuringQuest);
            }
            else if (playerNut.ratkilled >= 5)
            {
                CompleteQuest();
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
        }
    }

    private void StartDialogue(string[] dialogue)
    {
        if (dialogue.Length == 0) return;
        isTalking = true;
        dialoguePanel.SetActive(true);
        dialogueAnimator.SetBool("IsOpen", true);
        currentDialogueIndex = 0;
        ShowDialogueLine(dialogue);
    }

    private void ShowDialogueLine(string[] dialogue)
    {
        if (currentDialogueIndex < dialogue.Length)
        {
            dialogueText.text = dialogue[currentDialogueIndex];
            nameText.text = "Rat Hunter";
            iconImage.sprite = ratIcon;
            currentDialogueIndex++;
        }
        else
        {
            EndDialogue();
        }
    }

    public void ShowNextDialogue()
    {
        string[] activeDialogue = GetActiveDialogue();
        if (currentDialogueIndex < activeDialogue.Length)
        {
            ShowDialogueLine(activeDialogue);
        }
        else
        {
            EndDialogue();
        }
    }

    private string[] GetActiveDialogue()
    {
        if (!isQuestStarted) return dialogueStartQuest;
        if (!isQuestCompleted && playerNut.ratkilled < 5) return dialogueDuringQuest;
        return dialogueQuestComplete;
    }

    private void EndDialogue()
    {
        dialogueAnimator.SetBool("IsOpen", false);
        StartCoroutine(CloseDialogueAfterDelay());
    }

    private IEnumerator CloseDialogueAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        dialoguePanel.SetActive(false);
        isTalking = false;

        if (!isQuestStarted)
        {
            StartRatQuest();
        }
        else if (playerNut.ratkilled >= 5 && !isQuestCompleted)
        {
            CompleteQuest();
        }
    }

    private void StartRatQuest()
    {
        questManager.StartRatQuest();
        isQuestStarted = true;
        ratCollectorPanel.SetActive(true);
    }

    private void CompleteQuest()
    {
        playerNut.ratkilled = 0;
        isQuestCompleted = true;
        questManager.CompleteRatQuest();
        ratCollectorPanel.SetActive(false);
        StartDialogue(dialogueQuestComplete);
        ActivateEncyclopedia();
        EnableGameObjects();
        DisableGameObjects();
    }

    private void ActivateEncyclopedia()
    {
        encyclopediaButton.gameObject.SetActive(true);
        StartCoroutine(FadeInMessage("QUEST COMPLETE: Rat Hunt recorded in Encyclopedia"));
    }

    private IEnumerator FadeInMessage(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        float elapsedTime = 0f;
        Color color = messageText.color;
        color.a = 0f;
        messageText.color = color;

        while (elapsedTime < fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeInTime);
            messageText.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(messageDisplayTime);

        elapsedTime = 0f;
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutTime);
            messageText.color = color;
            yield return null;
        }

        messageText.gameObject.SetActive(false);
    }

    private void ValidateInspectorReferences()
    {
        if (!playerNut || !questManager || !interactPrompt || !dialoguePanel || !dialogueText || !nameText || !iconImage || !nextButton || !dialogueAnimator || !ratCollectorPanel || !encyclopediaButton || !messageText)
        {
            Debug.LogError("One or more references are missing in the Inspector!");
        }
    }

    public void EnableGameObjects()
    {
        foreach (GameObject obj in objectsToEnable)
        {
            obj?.SetActive(true);
        }
    }

    public void DisableGameObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            obj?.SetActive(false);
        }
    }
}
