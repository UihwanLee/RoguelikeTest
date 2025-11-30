using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProjectileInfo
{
    [field: SerializeField] public Sprite ProjectTileSprite { get; private set; }
    [field: SerializeField] public float ProjectileSpeed { get; private set; }
}

[CreateAssetMenu(fileName = "NewAdvisor", menuName = "Advisor/BaseAdvisor")]
public class AdvisorSO : ScriptableObject
{
    [field: Header("Base Info")]
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Desc { get; private set; }

    [field: Header("Condition")]

    [field: SerializeField] public float MaxHp { get; private set; }

    [field: Header("AttackInfo")]
    [field: SerializeField] public AttackInfo AttackInfo { get; private set; }
    [field: SerializeField] public ProjectileInfo ProjectileInfo { get; private set; }
}
