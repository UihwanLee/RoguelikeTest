using System;
using UnityEngine;

[Serializable]
public class AttackInfo
{
    [field: SerializeField] public LayerMask AttackTarget { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField][field:Range(0f, 5f)] public float AttackCoolTime { get; private set; }
}

[Serializable]
public class RewardInfo
{
    [field: SerializeField] public float GainExp { get; private set; }
    [field: SerializeField] public GameObject GoldPrefab { get; private set; }
}

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemy/BaseEnemy")]
public class EnemySO : ScriptableObject
{
    [field: Header("Base Info")]
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Desc { get; private set; }
    [field: SerializeField][field: Range(0f, 3f)] public float MoveSpeed { get; private set; }

    [field: Header("Condition")]
    
    [field: SerializeField] public float MaxHp { get; private set; }

    [field: Header("AttackInfo")]
    [field: SerializeField] public AttackInfo AttackInfo {  get; private set; }
    [field: Header("Spawn")]
    [field: SerializeField] public Sprite SpawnIcon { get; private set; }
    [field: SerializeField] public float MinSpawnDuration { get; private set; }
    [field: SerializeField] public float MaxSpawnDuration { get; private set; }

    [field: Header("RewardInfo")]
    [field: SerializeField] public RewardInfo RewardInfo { get; private set; }
}
