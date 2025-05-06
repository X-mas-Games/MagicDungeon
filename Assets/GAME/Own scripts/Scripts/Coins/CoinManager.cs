using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance; // Singleton instance to ensure only one manager exists
    
    private HashSet<GameObject> _coins = new HashSet<GameObject>(); // To keep track of all coins in the scene
    
    public CoinData CoinData; // Reference to data that stores the player coin count

    private void Awake()
    {
        // There is only one instance of "CoinManager"
        
        if (Instance == null)
        {
            Instance = this;
           
        }
       
    }

    private void Start()
    {
        UICoinManager.Instance?.UpdateCoinText(CoinData.CoinCount);  // Updates "UI" reflect the current number of coins
    }

    public void RegisterCoin(GameObject coin) // Registering new "coin" the game
    {
        _coins.Add(coin);
    }

    public void RemoveCoin(GameObject coin)  // Removes "coin" from the game when collected
    {
        if (_coins.Remove(coin) && _coins.Count == 0)
        {
            SceneLoader.Instance.LoadNextScene(); // If we have no more "coins" left, load the next scene
        }
    }

    public void CollectCoin() // Increments the "coin" count and updates the "UI"
    {
        CoinData.CoinCount++; // Increase the stored "coin" count
        
        UICoinManager.Instance.UpdateCoinText(CoinData.CoinCount); // Update "UI"
    }
}