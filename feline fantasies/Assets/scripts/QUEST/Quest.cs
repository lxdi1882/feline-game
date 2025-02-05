using UnityEngine;

public class Quest
{
    public string questName;
    public string questDescription;
    public bool isActive;
    public bool isCompleted;

    // Constructor to set quest name and description
    public Quest(string name, string description)
    {
        questName = name;
        questDescription = description;
        isActive = false;
        isCompleted = false;
    }

    // Method to start the quest
    public void StartQuest()
    {
        isActive = true;
        Debug.Log(questName + " started!");  // This will work now
    }

    // Method to complete the quest
    public void CompleteQuest()
    {
        isActive = false;
        isCompleted = true;
        Debug.Log(questName + " completed!");  // This will work now
    }
}
