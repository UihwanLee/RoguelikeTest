using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.InputSystem;

public class GoldSpawnManager : MonoBehaviour
{
    public static GoldSpawnManager Instance { get; private set; }

    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _spawnCount;
    private PoolManager _poolManager;
    private string _key;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _poolManager = PoolManager.Instance;
        GenerateProjectile();
    }

    private GoldSpawnManager() { }

    private void GenerateProjectile()
    {
        _key = _prefab.name;
        _poolManager.CreatePool(_key, _prefab, _spawnCount, this.transform);
    }

    public void SpawnGold(Vector3 position)
    {
        GameObject obj = _poolManager.GetObject(_key);
        obj.transform.position = position;
    }

    public void ReleaseObject(GameObject obj)
    {
        _poolManager.ReleaseObject(_key, obj);
    }
}
