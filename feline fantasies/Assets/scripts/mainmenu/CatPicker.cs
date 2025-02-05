using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CatPicker : MonoBehaviour
{
    [System.Serializable]
    public class CatData
    {
        public string name;
        public Sprite image;
        public string description;
        public GameObject selectButton;
        public bool isUnlocked = false;
        public bool isDefaultUnlocked = false;  // Mark as default unlocked cat
    }

    public Image catDisplay;
    public TextMeshProUGUI catNameText;
    public TextMeshProUGUI catDescriptionText;
    public GameObject descriptionPanel;
    public Button prevButton, nextButton;
    public Button unlockButton;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI coinText;  // TextMeshProUGUI for displaying the coin count

    public CatData[] cats;
    private int unlockCost = 20;
    private int currentIndex = 0;

    private const string UnlockKeyPrefix = "CatUnlocked_";

    public Button resetCatsButton;  // Reference to the reset button for cats

    void Start()
    {
        LoadData();

        if (cats.Length > 0)
        {
            UpdateCatDisplay();
        }

        prevButton.onClick.AddListener(PreviousCat);
        nextButton.onClick.AddListener(NextCat);
        unlockButton.onClick.AddListener(UnlockCat);

        // Add listener for the reset button
        resetCatsButton.onClick.AddListener(ResetCats);
    }

    void PreviousCat()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = cats.Length - 1;
        UpdateCatDisplay();
    }

    void NextCat()
    {
        currentIndex++;
        if (currentIndex >= cats.Length) currentIndex = 0;
        UpdateCatDisplay();
    }

    void UpdateCatDisplay()
    {
        CatData currentCat = cats[currentIndex];

        // Display the current coin count
        if (coinText != null)
        {
            coinText.text = "Coins: " + CoinManager.instance.coinCount;  // Display current coin count from CoinManager
        }

        if (currentCat.isUnlocked || currentCat.isDefaultUnlocked)
        {
            catDisplay.sprite = currentCat.image;
            catNameText.text = currentCat.name;
            catDescriptionText.text = currentCat.description;
            unlockButton.gameObject.SetActive(false);
            costText.gameObject.SetActive(false);
            catDisplay.color = Color.white;
        }
        else
        {
            catDisplay.sprite = currentCat.image;
            catNameText.text = "???";
            catDescriptionText.text = "Locked";
            unlockButton.gameObject.SetActive(true);
            costText.gameObject.SetActive(true);
            costText.text = "Unlock Cost: " + unlockCost + " Coins";
            catDisplay.color = new Color(0.5f, 0.5f, 0.5f, 1);
        }

        descriptionPanel.SetActive(true);

        foreach (var cat in cats)
        {
            cat.selectButton.SetActive(false);
        }

        if (currentCat.isUnlocked || currentCat.isDefaultUnlocked)
        {
            currentCat.selectButton.SetActive(true);
        }
    }

    public void UnlockCat()
    {
        if (CoinManager.instance != null && CoinManager.instance.SpendCoins(unlockCost))
        {
            cats[currentIndex].isUnlocked = true;
            UpdateCatDisplay();
            SaveData();
        }
    }

    public void SaveData()
    {
        foreach (var cat in cats)
        {
            // Save only the unlocked status for cats that are not default unlocked
            if (!cat.isDefaultUnlocked)
            {
                PlayerPrefs.SetInt(UnlockKeyPrefix + cat.name, cat.isUnlocked ? 1 : 0);
            }
        }
        PlayerPrefs.Save();
    }

    void LoadData()
    {
        for (int i = 0; i < cats.Length; i++)
        {
            // Load the unlocked state only for cats that are not default unlocked
            if (!cats[i].isDefaultUnlocked)
            {
                cats[i].isUnlocked = PlayerPrefs.GetInt(UnlockKeyPrefix + cats[i].name, 0) == 1;
            }
        }
    }

    public void ResetCats()
    {
        // Reset only unlockable cats, leave default unlocked cats as they are
        foreach (var cat in cats)
        {
            if (!cat.isDefaultUnlocked)
            {
                cat.isUnlocked = false;
                cat.selectButton.SetActive(false);  // Hide the select button for locked cats
            }
        }

        UpdateCatDisplay();  // Update the UI to reflect the reset data
        SaveData();  // Save the reset data to PlayerPrefs
    }
}
