using UnityEngine;

public class Coin : MonoBehaviour
{
    private void Start()
    {
        CoinManager.Instance?.RegisterCoin(gameObject); // Register this coin in "CoinManager"  when spawns
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player touched  coin on trigger
        {
            CoinManager.Instance.CollectCoin(); // Inform "CoinManager" that coin was collected
           
            CoinManager.Instance.RemoveCoin(gameObject);  // Remove the coin from tracking
            
            Destroy(gameObject);  // Destroy coin object
        }
    }
}