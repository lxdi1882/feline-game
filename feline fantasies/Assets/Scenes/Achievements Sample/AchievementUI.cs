using UnityEngine;
using System.Collections;
using TMPro;

public class AchievementUI : MonoBehaviour
{
    public TextMeshProUGUI AchievementText;
    public GameObject panel;

    private void Start()
    {
        panel.SetActive(false); // Hide at start
    }

    public void ShowAchievement(string questName)
    {
        panel.SetActive(true);
        AchievementText.text = $"Achievement Unlocked:\n{questName} Completed!";
        StartCoroutine(HideNotification());
    }

    private IEnumerator HideNotification()
    {
        yield return new WaitForSeconds(3f); // Show for 3 seconds
        panel.SetActive(false);
    }
}
