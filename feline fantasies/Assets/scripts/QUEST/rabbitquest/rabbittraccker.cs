using UnityEngine;

public class RabbitTracker : MonoBehaviour
{
    public BoxCollider2D detectionZone; // Reference to the BoxCollider2D in the Inspector
    private int rabbitCount = 0;        // Store the number of rabbits within the collider

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that enters the trigger zone has the "rabbit" tag
        if (other.CompareTag("Rabbit"))
        {
            rabbitCount++; // Increment the rabbit count
            Debug.Log("A rabbit has entered the zone. Total rabbits: " + rabbitCount);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the object that exits the trigger zone has the "rabbit" tag
        if (other.CompareTag("Rabbit"))
        {
            rabbitCount--; // Decrement the rabbit count
            Debug.Log("A rabbit has left the zone. Total rabbits: " + rabbitCount);
        }
    }

    private void Update()
    {
        // Output the current rabbit count in the debug log each frame
        Debug.Log("Current rabbits following you: " + rabbitCount);
    }
}
