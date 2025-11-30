using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] private LayerMask goldMask;

    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (goldMask.value == (goldMask.value | (1 << collision.gameObject.layer)))
        {
            _player.Condition.AddCondition(ConditionType.Gold, 10);
            _player.FloatingTextPoolManager.SpawnText(TextType.Damage, $"+{10}", collision.transform, UnityEngine.Color.green);
            GoldSpawnManager.Instance.ReleaseObject(collision.gameObject);
        }
    }
}
