using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    private void Start()
    {
        CoinPool.Instance.ShowAllCoins();
    }
}