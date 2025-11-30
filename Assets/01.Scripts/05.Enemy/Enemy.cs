using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [field: SerializeField] public EnemySO Data { get; private set; }

    public SpriteRenderer SpriteRenderer { get; private set; }
    public EnemyController Controller { get; private set; }
    public EnemyCondition Condition { get; private set; }

    public EnemySpawnManager SpawnManager { get; private set; }
    public FloatingTextPoolManager FloatingTextPoolManager { get; private set; }

    private Coroutine _spawnCoroutine;

    private void Awake()
    {
        Controller = GetComponent<EnemyController>();
        Condition = GetComponent<EnemyCondition>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        Controller.Init(this);

        SpawnManager = EnemySpawnManager.Instacne;
        FloatingTextPoolManager = FloatingTextPoolManager.Instance;
    }

    public void Spawn(Vector3 randomPosition)
    {
        if (_spawnCoroutine != null) StopCoroutine(_spawnCoroutine);

        float spawnDuration = Random.Range(Data.MinSpawnDuration, Data.MaxSpawnDuration);
        _spawnCoroutine = StartCoroutine(SpawnCoroutine(randomPosition, spawnDuration));
    }

    private IEnumerator SpawnCoroutine(Vector3 randomPosition, float spawnDuration)
    {
        float curTime = 0f;

        Controller.enabled = false;
        Condition.enabled = false;
        transform.position = randomPosition;

        // Sprite 교체
        Sprite originSprite = SpriteRenderer.sprite;
        SpriteRenderer.sprite = Data.SpawnIcon;

        Color color = SpriteRenderer.color;

        while (curTime < spawnDuration)
        {
            curTime += Time.deltaTime;

            float t = curTime / spawnDuration;

            color.a = t;

            SpriteRenderer.color = color;

            yield return null;
        }

        color.a = 1f;
        SpriteRenderer.color = color;
        SpriteRenderer.sprite = originSprite;
        Controller.enabled = true;
        Condition.enabled = true;
    }
}
