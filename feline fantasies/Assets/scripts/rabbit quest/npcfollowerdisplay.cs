using UnityEngine;
using TMPro; // Required for TextMeshPro

public class NPCFollowerDisplay : MonoBehaviour
{
    public TextMeshProUGUI followingCountText; // Reference to the TextMeshPro UI element

    private void Update()
    {
        // Update the text to display the current following count
        if (followingCountText != null)
        {
            followingCountText.text = "* " + NPCFollower.followingCount.ToString() + "/4 Rabbits" ; 
        }
        if (NPCFollower.followingCount >= 4)
        {
            followingCountText.text = "* Return all Rabbits";
        }

    }
}