using UnityEngine;
using UnityEngine.Serialization;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] private Transform _teleportDestination;
    [SerializeField] private Vector3 _teleportOffset = Vector3.zero; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Teleport(other);
        }
    }

    private void Teleport(Collider player)
    {
        if (_teleportDestination == null)
        {
            Debug.LogError("Teleport destination not assigned", this);
            return;
        }

        CharacterController controller = player.GetComponent<CharacterController>();

        Vector3 targetPosition = _teleportDestination.position + _teleportOffset;

        if (controller != null)
        {
            controller.enabled = false; // Disable CharacterController to avoid physics issues
            player.transform.position = targetPosition;
            controller.enabled = true; // Re-enable it after teleportation
        }
        else
        {
            player.transform.position = targetPosition;
        }

      
    }
}