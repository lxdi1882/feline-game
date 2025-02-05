using UnityEngine;

public class food : MonoBehaviour
{
    private PlayerNutCollection player;  // Reference to PlayerNutCollection script

    void Start()
    {
        // Find the PlayerNutCollection script attached to the player
        player = FindObjectOfType<PlayerNutCollection>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && player != null)
        {
            player.CollectFood();  // Call the player’s CollectNut method
            Destroy(gameObject);  // Destroy the nut after collecting
        }
    }
}
