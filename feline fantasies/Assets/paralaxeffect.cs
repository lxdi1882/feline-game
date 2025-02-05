using UnityEngine;

public class LoopingParallaxEffect : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f; // How fast the background moves
    public float movementRange = 10f; // How far it moves left and right

    private Vector3 startPosition; // Original position of the panel

    void Start()
    {
        // Record the initial position of the panel
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate horizontal movement based on time
        float offsetX = Mathf.Sin(Time.time * speed) * movementRange;

        // Apply the calculated position
        transform.position = startPosition + new Vector3(offsetX, 0, 0);
    }
}
