using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ObjectPool을 모아놓은 PoolManager 스크립트
/// ObjectPool을 Dictionary 자료구조로 관리
/// </summary>
public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private Dictionary<string, ObjectPool> _objectPools = new Dictionary<string, ObjectPool>();

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

    private PoolManager() { }

    /// <summary>
    /// ObjectPool 생성
    /// </summary>
    /// <param name="key">Pool Key</param>
    /// <param name="prefab">Pool 오브젝트</param>
    /// <param name="initialSize">오브젝트 생성 개수</param>
    /// <param name="parent">오브젝트 생성 부모</param>
    public void CreatePool(string key, GameObject prefab, int initialSize, Transform parent = null)
    {
        // Dictionary에 존재하는지 확인
        if(_objectPools.ContainsKey(key))
        {
            ($"해당 {key}를 가지고 있는 ObjectPool이 이미 존재합니다.").EditorLog();
        }

        Transform newParent = parent;

        if(newParent == null)
        {
            // 부모 오브젝트 생성
            GameObject poolParent = new GameObject($"Pool_{key}");
            poolParent.transform.SetParent(this.transform);

            newParent = poolParent.transform;
        }

        // ObjectPool 생성
        ObjectPool pool = new ObjectPool(prefab, initialSize, newParent);
        _objectPools.Add(key, pool);
    }

    /// <summary>
    /// ObjectPool에서 Object 가져오기
    /// </summary>
    /// <param name="key">Pool Key</param>
    /// <returns>Pool Object</returns>
    public GameObject GetObject(string key)
    {
        // Dictionary에서 해당 key에 맞는 object를 가져옴
        if(_objectPools.TryGetValue(key, out ObjectPool pool))
        {
            return pool.Get();
        }

        ($"해당 {key}값을 가지고 있는 ObjectPool이 존재하지 않습니다").EditorLog();
        return null;
    }

    /// <summary>
    /// ObjectPool에 Object 반납
    /// </summary>
    /// <param name="key">Pool Key</param>
    /// <param name="obj">반납할 Object</param>
    public void ReleaseObject(string key, GameObject obj)
    {
        // Dicitonay에서 해당 key에 맞는 Pool 가져옴
        if(_objectPools.TryGetValue(key, out ObjectPool pool))
        {
            pool.Release(obj);
        }
        else
        {
            // Pool 없다면 오브젝트를 강제로 비활성화하여 디버그 출력
            if(obj != null)
            {
                obj.gameObject.SetActive(false);
            }

            ($"해당 {key}값을 가지고 있는 ObjectPool이 존재하지 않습니다").EditorLog();
        }
    }
}
