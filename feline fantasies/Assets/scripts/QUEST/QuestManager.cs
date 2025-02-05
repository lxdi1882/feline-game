using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests = new List<Quest>();  // List of all quests

    void Start()
    {
        quests.Add(new Quest("Beaver Quest", "Help the beaver collect nuts"));
    }


    public void CompleteFetchQuest()
    {
        // Find the "Fetch Nuts" quest and mark it as completed
        Quest fetchQuest = quests.Find(q => q.questName == "Fetch Nuts");
        if (fetchQuest != null && fetchQuest.isActive)
        {
            Debug.Log("Fetch Quest Completed!");
        }
    }
    public void StartFetchQuest()
    {
        Quest fetchQuest = quests.Find(q => q.questName == "Fetch Nuts");
        if (fetchQuest != null && !fetchQuest.isActive)
        {
            fetchQuest.StartQuest();
            Debug.Log("Quest Started: Fetch Nuts");
        }
    }

    public void CompleteBeaverQuest()
    {
        // Find the "Fetch Nuts" quest and mark it as completed
        Quest beaverQuest = quests.Find(q => q.questName == "BeaverQuest");
        if (beaverQuest != null && beaverQuest.isActive)
        {
            Debug.Log("Beaver Quest Completed!");
        }
    }
    public void StartBeaverQuest()
    {
        Quest beaverQuest = quests.Find(q => q.questName == "beaverQuest");
        if (beaverQuest != null && !beaverQuest.isActive)
        {
            beaverQuest.StartQuest();
            Debug.Log("Quest Started: Beaver Quest");
        }
    }
    public void CompleteFoxQuest()
    {
        // Find the "Fetch Nuts" quest and mark it as completed
        Quest foxQuest = quests.Find(q => q.questName == "foxQuest");
        if (foxQuest != null && foxQuest.isActive)
        {
            Debug.Log("fox Quest Completed!");
        }
    }
    public void StartFoxQuest()
    {
        Quest foxQuest = quests.Find(q => q.questName == "foxQuest");
        if (foxQuest != null && !foxQuest.isActive)
        {
            foxQuest.StartQuest();
            Debug.Log("Quest Started: foxQuest");
        }
    }
    public void CompleteBunnyQuest()
    {
        // Find the "Fetch Nuts" quest and mark it as completed
        Quest bunnyQuest = quests.Find(q => q.questName == "bunnyQuest");
        if (bunnyQuest != null && bunnyQuest.isActive)
        {
            Debug.Log("bunny Quest Completed!");
        }
    }
    public void StartBunnyQuest()
    {
        Quest bunnyQuest = quests.Find(q => q.questName == "bunnyQuest");
        if (bunnyQuest != null && !bunnyQuest.isActive)
        {
            bunnyQuest.StartQuest();
            Debug.Log("Quest Started: bunnyQuest");
        }
    }
}

   