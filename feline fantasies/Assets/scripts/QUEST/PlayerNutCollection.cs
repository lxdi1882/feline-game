using UnityEngine;
using TMPro;  // Import the TextMeshPro namespace

public class PlayerNutCollection : MonoBehaviour
{
    public QuestManager questManager;  // Reference to the QuestManager
    private CoinManager coinManager;  // Reference to the CoinManager (dynamically found)
    public int nutsCollected = 0;  // Keep track of collected nuts
    private bool isNearSquirrel = false;  // Check if player is near the squirrel
    public TextMeshProUGUI nutsCollectedText;  // Reference to the TextMeshProUGUI component
    public AudioSource collectSound;  // Reference to the AudioSource for the nut collection sound
    public int foodsCollected = 0;
    public int woodsCollected = 0;
    public TextMeshProUGUI foodCollectedText;
    public TextMeshProUGUI woodCollectedText;
    public TextMeshProUGUI CoinCollectedText;

    void Start()
    {
        // Dynamically find the CoinManager instance in the scene
        coinManager = FindObjectOfType<CoinManager>();

        // Ensure CoinManager is found
        if (coinManager == null)
        {
            Debug.LogError("CoinManager not found in the scene. Make sure it is present.");
        }

        UpdateNutsCollectedUI();  // Initialize the UI with the starting count
        UpdateCoinCollectedUI();  // Initialize the coin UI
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Squirrel"))
        {
            isNearSquirrel = true;  // Player is near the squirrel
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Squirrel"))
        {
            isNearSquirrel = false;  // Player has left the squirrel's area
        }
    }

    // Method for collecting nuts
    public void CollectNut()
    {
        nutsCollected++;
        Debug.Log("Nuts Collected: " + nutsCollected);  // Log collected nuts
        UpdateNutsCollectedUI();  // Update the UI to show the new count
        PlayCollectSound();  // Play the sound when a nut is collected
    }

    // Method to play the nut collection sound
    private void PlayCollectSound()
    {
        if (collectSound != null)
        {
            collectSound.Play();  // Play the audio clip attached to the AudioSource
        }
    }

    // Method to update the TextMeshProUGUI text
    private void UpdateNutsCollectedUI()
    {
        if (nutsCollectedText != null)
        {
            nutsCollectedText.text = nutsCollected + "/5 nuts collected ";
        }
        if (nutsCollected >= 5)
        {
            nutsCollectedText.text = "Give nuts to Squirrel";
        }
    }

    public void CollectFood()
    {
        foodsCollected++;
        Debug.Log("Food Collected: " + foodsCollected);  // Log collected food
        UpdateFoodCollectedUI();  // Update the UI to show the new count
        PlayCollectSound();  // Play the sound when a food is collected
    }

    private void UpdateFoodCollectedUI()
    {
        if (foodCollectedText != null)
        {
            foodCollectedText.text = foodsCollected + "/4 Collected Grapes ";
        }
        if (foodsCollected >= 4)
        {
            foodCollectedText.text = "Give grapes to Fox";
        }
    }

    public void CollectWood()
    {
        woodsCollected++;
        Debug.Log("Wood Collected: " + woodsCollected);  // Log collected wood
        UpdateWoodCollectedUI();  // Update the UI to show the new count
        PlayCollectSound();  // Play the sound when wood is collected
    }

    private void UpdateWoodCollectedUI()
    {
        if (woodCollectedText != null)
        {
            woodCollectedText.text = woodsCollected + "/6 Collected Wood";
        }
        if (woodsCollected >= 6)
        {
            woodCollectedText.text = "Give wood to Beaver";
        }
    }

    // Collect coins and update UI using CoinManager
    public void CollectCoin()
    {
        if (coinManager != null)
        {
            coinManager.AddCoins(10);  // Add coins through CoinManager
            UpdateCoinCollectedUI();  // Update the UI with the new coin count
        }
        else
        {
            Debug.LogError("CoinManager is not assigned or not found.");
        }

        PlayCollectSound();  // Play the sound when a coin is collected
    }

    private void UpdateCoinCollectedUI()
    {
        if (CoinCollectedText != null && coinManager != null)
        {
            CoinCollectedText.text = coinManager.coinCount.ToString();  // Display the coin count from CoinManager
        }
    }
}
