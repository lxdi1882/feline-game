using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f; // How close the player must be to interact
    public LayerMask interactableLayer; // Layer for interactable objects

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Check if the player presses E
        {
            InteractWithObject();
        }
    }

    private void InteractWithObject()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRange, interactableLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Tree")) // Check if the object is tagged as Tree
            {
                TreeInteraction treeInteraction = hit.GetComponent<TreeInteraction>();
                if (treeInteraction != null)
                {
                    treeInteraction.InteractWithTree(); // Trigger tree interaction
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange); // Visualize interaction range
    }
}
