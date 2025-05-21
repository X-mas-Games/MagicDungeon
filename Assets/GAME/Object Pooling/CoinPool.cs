using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CoinPool : MonoBehaviour
{
    public static CoinPool Instance;

    [FormerlySerializedAs("CoinPrefab")]
    [Header("Монеты и Пул")]
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private int _initialPoolSize = 50;
    [SerializeField] private  float _respawnDelay = 5f;
    
    [Header("Зоны Появления")]
    [SerializeField] private  Transform[] _spawnZones; 

    private List<GameObject> Pool = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        CreateInitialPool();
    }

    private void CreateInitialPool()
    {
        for (int i = 0; i < _initialPoolSize; i++)
        {
            CreateCoin();
        }
    }

    private GameObject CreateCoin()
    {
        GameObject coin = Instantiate(_coinPrefab);
        coin.SetActive(false);
        Pool.Add(coin);
        return coin;
    }

    public void ShowAllCoins()
    {
        foreach (GameObject coin in Pool)
        {
            if (!coin.activeInHierarchy)
            {
                coin.transform.position = GetRandomSpawnPosition();
                coin.SetActive(true);
            }
        }
    }

    public void ReturnCoin(GameObject coin)
    {
        coin.SetActive(false);
        StartCoroutine(RespawnCoinAfterDelay(coin));
    }

    private IEnumerator RespawnCoinAfterDelay(GameObject coin)
    {
        yield return new WaitForSeconds(_respawnDelay);
        coin.transform.position = GetRandomSpawnPosition();
        coin.SetActive(true);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        if (_spawnZones == null || _spawnZones.Length == 0)
        {
            Debug.LogWarning("SpawnZones не назначены! Используется позиция (0,0,0)");
            return Vector3.zero;
        }

        Transform zone = _spawnZones[Random.Range(0, _spawnZones.Length)];
        Vector3 center = zone.position;
        float range = 2f; // Разброс вокруг центра зоны

        return new Vector3(
            center.x + Random.Range(-range, range),
            center.y,
            center.z + Random.Range(-range, range)
        );
    }

    public GameObject GetCoin()
    {
        foreach (GameObject coin in Pool)
        {
            if (!coin.activeInHierarchy)
                return coin;
        }

        // Авторасширение пула
        return CreateCoin();
    }
}
