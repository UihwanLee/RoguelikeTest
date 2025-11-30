using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Rect _spawnAreas;
    [SerializeField] private int _spawnCount = 20;

    [SerializeField] private bool OnDrawGizomo = true;

    private PoolManager _poolManager;
    private Enemy _enemy;
    private string _key;

    private void Start()
    {
        _poolManager = PoolManager.Instance;
        _enemy = _prefab.transform.GetComponent<Enemy>();
        _key = _enemy.Data.Name;

        GenerateEnemy();
    }

    private void GenerateEnemy()
    {
        _poolManager.CreatePool(_key, _enemy.gameObject, _spawnCount, this.transform);
    }

    public void Spawn()
    {
        Enemy enemy = _poolManager.GetObject(_enemy.Data.Name).GetComponent<Enemy>();

        if(enemy != null && enemy.gameObject.activeSelf)
            enemy.Spawn(GetRandomPosition());
    }

    private Vector2 GetRandomPosition()
    {
        Rect randomArea = _spawnAreas;

        Vector2 randomPosition = new Vector2(
            Random.Range(randomArea.xMin, randomArea.xMax),
            Random.Range(randomArea.yMin, randomArea.yMax));

        return randomPosition;
    }

    public void Release(GameObject obj)
    {
        _poolManager.ReleaseObject(Key, obj);
    }

    public string Key { get { return _key; } }

    private void OnDrawGizmos()
    {
        if (OnDrawGizomo == false) return;

        Gizmos.color = Color.green;

        Vector3 center = new Vector3(_spawnAreas.x + _spawnAreas.width / 2, _spawnAreas.y + _spawnAreas.height / 2);
        Vector3 size = new Vector3(_spawnAreas.width, _spawnAreas.height);

        Gizmos.DrawCube(center, size);
    }
}
