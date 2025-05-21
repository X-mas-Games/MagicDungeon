using UnityEngine;

public class CoinsTrigger : MonoBehaviour
{
     private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CoinPool.Instance.ReturnCoin(gameObject);
        }
    }
}
