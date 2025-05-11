using UnityEngine;
using System.Collections;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] private Transform _teleportDestination;
    [SerializeField] private Vector3 _teleportOffset = Vector3.zero;
    [SerializeField] private float _teleportDelay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TeleportAfterDelay(other));
        }
    }

    private IEnumerator TeleportAfterDelay(Collider player) // Coroutine that waits a specified delay before teleporting
    {
        if (_teleportDelay > 0f)
            yield return new WaitForSeconds(_teleportDelay);

        Teleport(player);
    }

  
    private void Teleport(Collider player)
    {
        if (!_teleportDestination)
        {
            Debug.LogError("Teleport destination not assigned", this);
            return;
        }
               
        Vector3 targetPosition = _teleportDestination.position + _teleportOffset;  // Calculate initial position the teleport

        if (player.TryGetComponent(out CharacterController controller))
        {
            controller.enabled = false;
            player.transform.position = targetPosition;
            controller.enabled = true;
        }
        else
        {
            player.transform.position = targetPosition;
        }
    }
}