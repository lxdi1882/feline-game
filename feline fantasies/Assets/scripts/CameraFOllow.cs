using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // The target (your cat/player)
    public float smoothTime = 0.3f;  // Time to smooth out movement
    public Vector3 offset;  // The offset from the target position (e.g., camera height)

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        // Calculate the desired position with the offset
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);

        // Smoothly move the camera towards the desired position using SmoothDamp
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }
}
