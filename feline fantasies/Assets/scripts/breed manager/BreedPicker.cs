using UnityEngine;
using UnityEngine.UI;

public class BreedPickerUI : MonoBehaviour
{
    public Button[] breedButtons; // List of buttons for breed selection
    public BreedManager breedManager; // Reference to the BreedManager script

    void Start()
    {
        // Set up buttons to select breeds
        for (int i = 0; i < breedButtons.Length; i++)
        {
            int index = i; // Capture the correct index for the button
            breedButtons[i].onClick.AddListener(() => OnBreedSelected(index));
        }
    }

    void OnBreedSelected(int breedIndex)
    {
        breedManager.SetBreed(breedIndex); // Set the selected breed in BreedManager
    }
}
