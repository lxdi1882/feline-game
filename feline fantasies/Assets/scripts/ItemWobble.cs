using UnityEngine;

public class ItemWobble : MonoBehaviour
{
    public float wobbleSpeed = 2f; // Speed of the wobble
    public float wobbleHeight = 0.2f; // Height of the wobble
    public float rotationSpeed = 30f; // Speed of the rotation

    private Vector3 startPosition; // Initial position of the item
    private float wobbleOffset; // Random offset for unique wobble behavior

    void Start()
    {
        // Save the item's initial position
        startPosition = transform.position;

        // Generate a random offset for the wobble
        wobbleOffset = Random.Range(0f, Mathf.PI * 2);
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave with a random offset
        float newY = startPosition.y + Mathf.Sin(Time.time * wobbleSpeed + wobbleOffset) * wobbleHeight;

        // Apply the new position
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // Rotate the item slightly over time
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
