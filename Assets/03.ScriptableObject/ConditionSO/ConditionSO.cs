using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConditionData", menuName = "Data/Condition")]
public class ConditionInfoSO : ScriptableObject
{
    [field: Header("Exp")]
    [field: SerializeField] public float BaseMaxExp { get; private set; }
    [field: SerializeField][field: Range(1f, 2f)] public float ExpIncreaseScaling { get; private set; }

    [field: Header("Hp")]
    [field: SerializeField] public float BaseMaxHp { get; private set; }
    [field: SerializeField][field: Range(1f, 2f)] public float HpIncreaseScaling { get; private set; }

    [field: Header("Atk")]
    [field: SerializeField] public float BaseAtk { get; private set; }
    [field: SerializeField][field: Range(1f, 2f)] public float AtkIncreasScaling { get; private set; }
    [field: SerializeField] public float KnockbackPower { get; private set; }

    [field: Header("Speed")]
    [field: SerializeField] public float BaseSpeed { get; private set; }
    [field: SerializeField][field: Range(1f, 2f)] public float SpeedIncreasScaling { get; private set; }

    #region Player SO

    [field: Header("Player")]
    [field: SerializeField] public int FlashDamageCount { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float InvincibilitySpeedMulitplier { get; private set; }
    [field: SerializeField][field: Range(0f, 5f)] public float InvincibilityDuration { get; private set; }

    #endregion
}
