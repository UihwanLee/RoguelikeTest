using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Advisor : MonoBehaviour
{
    [field: SerializeField] public AdvisorSO Data { get; private set; }

    public AdvisorController Controller { get; private set; }
    public AdvisorCondition Condition { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }

    public bool OnDrawGizomo = false;

    public ProjectileManager ProjectileManager { get; private set; }

    private void Awake()
    {
        Controller = GetComponent<AdvisorController>();
        Condition = GetComponent<AdvisorCondition>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        Controller.Init(this);

        ProjectileManager = ProjectileManager.Instance;
    }

    private void OnDrawGizmos()
    {
        if(OnDrawGizomo)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, Data.AttackInfo.AttackRange);
        }
    }
}
