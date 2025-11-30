using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int spawnCount = 50;

    private PoolManager _poolManager;
    private const string KEY = "Projectile";

    public static ProjectileManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
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

    private ProjectileManager() { }

    private void GenerateProjectile()
    {
        _poolManager.CreatePool(KEY, projectilePrefab, spawnCount, this.transform);
    }

    public void ShootProjectile(Advisor advisor, Vector3 position, Vector3 direction)
    {
        GameObject obj = _poolManager.GetObject(KEY);

        if(obj.TryGetComponent<ProjectileController>(out ProjectileController projectile))
        {
            projectile.Initialize(advisor, position, direction, this);
        }
    }

    public void ReleaseProjectile(GameObject projectile)
    {
        _poolManager.ReleaseObject(KEY, projectile);
    }
}
