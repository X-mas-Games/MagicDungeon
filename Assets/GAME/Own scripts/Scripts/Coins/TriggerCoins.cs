using UnityEngine;


public class TriggerCoins : MonoBehaviour
    
    
{
    [SerializeField] private GameObject _coin; 
    
    
    private void OnTriggerEnter(Collider other) // When the player or ground enter the trigger set shouldMove true
    {
        if (other.CompareTag("Player")) ;
        Destroy(_coin);



    }
}
