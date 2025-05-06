using UnityEngine;
using TMPro;
using UnityEngine;

public class UICoinManager : MonoBehaviour
{
    public static UICoinManager Instance; // Singleton instance to ensure only one UI manager exists
    
    public TextMeshProUGUI CoinText; // Reference to the "TextMeshPro" UI element displaying the coin count

    private void Awake()
    {
        // Ensure only one instance exists
        
        if (Instance == null)
        {
            Instance = this;
        }
        
    }

    public void UpdateCoinText(int coinCount) // Updates the "coin" text UI
    {
        if (CoinText != null) // Ensure text component exists
        {
            CoinText.text = coinCount.ToString(); // Convert int to string
        }
    }
}
