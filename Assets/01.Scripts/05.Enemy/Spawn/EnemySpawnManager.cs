using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instacne {  get; private set; }

    private EnemySpawn[] enemySpawns;

    [SerializeField] private float minSpawnCoolTime = 0.2f;
    [SerializeField] private float maxSpawnCoolTime = 4.0f;

    private float spawnCoolTime;
    private float currentCoolTime;

    private void Awake()
    {
        if(Instacne == null)
        {
            Instacne = this;
            DontDestroyOnLoad(Instacne);
        }
        else
        {
            Destroy(gameObject);
        }

        spawnCoolTime = Random.Range(minSpawnCoolTime, maxSpawnCoolTime);
    }

    private EnemySpawnManager() { }

    private void Start()
    {
        enemySpawns = GetComponentsInChildren<EnemySpawn>();
    }

    private void Update()
    {
        RandomSpawnEnemy();
    }

    private void RandomSpawnEnemy()
    {
        currentCoolTime += Time.deltaTime;

        if(currentCoolTime > spawnCoolTime)
        {
            currentCoolTime = 0.0f;
            spawnCoolTime = Random.Range(minSpawnCoolTime, maxSpawnCoolTime);
            EnemySpawn enemySpawn = enemySpawns[Random.Range(0, enemySpawns.Length)];
            enemySpawn.Spawn();
        }
    }

    public void SpawnEnemy(string key)
    {
        foreach(var enemySpawn in enemySpawns)
        {
            if(enemySpawn.Key == key)
            {
                enemySpawn.Spawn();
                return;
            }
        }
    }

    public void Release(string key, GameObject obj)
    {
        foreach (var enemySpawn in enemySpawns)
        {
            if (enemySpawn.Key == key)
            {
                enemySpawn.Release();
                return;
            }
        }
    }
}
