using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CoinPool : MonoBehaviour
{
    public static CoinPool Instance;
    private int _currentSpawnIndex = 0;
    
    [Header("Монеты и Пул")]
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private int _initialPoolSize ;
    [SerializeField] private  float _respawnDelay ;
    
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
                coin.transform.position = GetNextSpawnPosition();
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
        coin.transform.position = GetNextSpawnPosition();
        coin.SetActive(true);
    }

    private Vector3 GetNextSpawnPosition()
    {
        if (_spawnZones == null || _spawnZones.Length == 0)
        {
            Debug.LogWarning("SpawnZones не назначены! Используется позиция (0,0,0)");
            return Vector3.zero;
        }

        Transform zone = _spawnZones[_currentSpawnIndex];
        _currentSpawnIndex = (_currentSpawnIndex + 1) % _spawnZones.Length; // increases the index by 1 and resets it if it is out of bounds of the array

        return zone.position;
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
