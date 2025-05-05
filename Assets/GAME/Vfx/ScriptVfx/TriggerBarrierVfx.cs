using UnityEngine;

public class TriggerBarrierVfx : MonoBehaviour
{

    [SerializeField] GameObject _bricksEffectPrefab;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            SpawnEffectAndDestroy();
        }
    }


    private void SpawnEffectAndDestroy()
    {
        if (_bricksEffectPrefab != null)
        {
            Instantiate(_bricksEffectPrefab, transform.position, transform.rotation);
        }

        Destroy(gameObject);

    }
}