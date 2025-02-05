using UnityEngine;
using UnityEngine.UI;  // Add this line to fix the Button reference

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance; // Singleton instance for global access
    private const string CoinKey = "CoinCount"; // Save key

    public int coinCount = 0;  // The coin count, no UI handling here
    public int defaultCoinCount = 50;  // The default coin count when resetting

    public Button resetCoinsButton;  // Reference to the reset coins button

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }
    }

    void Start()
    {
        LoadCoins();  // Load coins from PlayerPrefs when the game starts

        // Add listener for the reset button
        if (resetCoinsButton != null)
        {
            resetCoinsButton.onClick.AddListener(ResetCoins);
        }
    }

    public void AddCoins(int amount)
    {
        coinCount += amount;
        SaveCoins();
    }

    public bool SpendCoins(int amount)
    {
        if (coinCount >= amount)
        {
            coinCount -= amount;
            SaveCoins();
            return true;
        }
        return false;
    }

    public void ResetCoins()
    {
        coinCount = defaultCoinCount;  // Reset coin count to the default value
        SaveCoins();  // Save the reset coin count to PlayerPrefs
    }

    void SaveCoins()
    {
        PlayerPrefs.SetInt(CoinKey, coinCount);
        PlayerPrefs.Save();
    }

    void LoadCoins()
    {
        coinCount = PlayerPrefs.GetInt(CoinKey, defaultCoinCount);  // Default to 50 coins if no data exists
    }
}
