using UnityEngine;
using TMPro;

public class ItemCollector : MonoBehaviour
{
    private int twigCount = 0;
    private int leafCount = 0;
    private int featherCount = 0;

    public TextMeshProUGUI twigCountText;
    public TextMeshProUGUI leafCountText;
    public TextMeshProUGUI featherCountText;

    public AudioClip itemPickupSound; // Assign your audio clip here
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component attached to the player
        audioSource = GetComponent<AudioSource>();

        // Initialize the UI with default counts
        UpdateUI();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectibles"))
        {
            string itemName = other.gameObject.name;

            // Check the name of the collectible and update counts
            if (itemName.Contains("Twig"))
            {
                twigCount++;
            }
            else if (itemName.Contains("Leaf"))
            {
                leafCount++;
            }
            else if (itemName.Contains("Feather"))
            {
                featherCount++;
            }

            // Update the UI
            UpdateUI();

            // Play the pickup sound
            audioSource.PlayOneShot(itemPickupSound);

            // Destroy the collectible after it's picked up
            Destroy(other.gameObject);
        }
    }

    void UpdateUI()
    {
        // Update the text fields with the current counts
        twigCountText.text = twigCount.ToString();
        leafCountText.text = leafCount.ToString();
        featherCountText.text = featherCount.ToString();
    }
}
