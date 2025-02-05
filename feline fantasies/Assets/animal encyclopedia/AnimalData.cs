using UnityEngine;

[CreateAssetMenu(fileName = "New Animal Data", menuName = "Animal Data")]
public class AnimalData : ScriptableObject
{
    public string animalName;
    public string description;
    public Sprite animalIcon;
    // Add other properties as needed
}
