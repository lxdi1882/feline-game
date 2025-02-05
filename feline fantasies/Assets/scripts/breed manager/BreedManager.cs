using UnityEngine;

public class BreedManager : MonoBehaviour
{
    public static RuntimeAnimatorController selectedAnimator; // Store the selected breed's animator

    public RuntimeAnimatorController[] breedAnimators; // Array of different breed animator controllers
    private int currentBreedIndex = 0; // Index to track the selected breed

    void Start()
    {
        // If we have no selected animator yet, set the first one by default
        if (selectedAnimator == null && breedAnimators.Length > 0)
        {
            selectedAnimator = breedAnimators[currentBreedIndex];
        }
    }

    // Call this method when a user selects a breed (e.g., on a button click in the menu)
    public void SetBreed(int breedIndex)
    {
        if (breedIndex >= 0 && breedIndex < breedAnimators.Length)
        {
            selectedAnimator = breedAnimators[breedIndex];
            Debug.Log("Breed selected: " + breedAnimators[breedIndex].name);
        }
    }

    // Optional: Add functionality to cycle through breeds if needed
    public void CycleBreed()
    {
        currentBreedIndex = (currentBreedIndex + 1) % breedAnimators.Length;
        SetBreed(currentBreedIndex);
    }
}
