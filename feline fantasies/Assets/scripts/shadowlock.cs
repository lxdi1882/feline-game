using UnityEngine;

public class ShadowLock : MonoBehaviour
{
    private Vector3 originalPosition;

    void Start()
    {
        // Store the shadow's initial position
        originalPosition = transform.position;
    }

    void LateUpdate()
    {
        // Lock the shadow's position to its original position
        transform.position = originalPosition;
    }
}
