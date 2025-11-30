using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AdvisorController : MonoBehaviour
{
    private Advisor _advisor;
    private BehaviorTree _tree;
    private Enemy _target;
    private Player _player;
    private bool _isCanAttack = false;
    private float _currentAttackCoolTime = 0f;

    private void Awake()
    {

    }

    public void Init(Advisor advisor)
    {
        _advisor = advisor;

        _player = GameManager.Instance.Player;

        SetBehavior();
    }

    private void SetBehavior()
    {
        // 리프 노드 생성
        ActionNode followPlayer = new ActionNode("Follow Player", FollowPlayer);
        ActionNode findNearEnemy = new ActionNode("Find Enemy By OverlapSphere", FindNearEnemy);
        ConditionNode isCanAttack = new ConditionNode("Is Can Attack?", IsCanAttack);
        ActionNode attackEnemy = new ActionNode("Attack Enemy", AttackEnemy);

        // 복합 노드 생성
        SequenceNode findAndAttackEnemy = new SequenceNode("Find And Attack Enemy", findNearEnemy, isCanAttack, attackEnemy);

        SequenceNode root = new SequenceNode("Advisor Root", findAndAttackEnemy);

        // BehaviorTree 생성
        _tree = new BehaviorTree(root);
    }

    private NodeState FollowPlayer()
    {
        // 타켓 위치로 이동 : AttackRange까지 접근
        if (Vector3.Distance(transform.position, _player.transform.position) < 0.2f)
        {
            return NodeState.SUCCESS;
        }

        // Target 위치로 Slerp 
        transform.position = Vector3.Slerp(transform.position, _player.transform.position, _advisor.Condition.Speed * Time.deltaTime);

        // Target과의 거리에 따른 Flip
        _advisor.SpriteRenderer.flipX = (transform.position.x - _player.transform.position.x) > 0;

        return NodeState.RUNNING;
    }

    private NodeState FindNearEnemy()
    {
        float searchRadius = _advisor.Data.AttackInfo.AttackRange;
        LayerMask targetLayer = _advisor.Data.AttackInfo.AttackTarget;

        // AttackRange에서 가까이 있는 Enemy 찾기
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, searchRadius, targetLayer);

        float shortestDistanceSqr = searchRadius * searchRadius;

        foreach (Collider2D hitCollider in hitColliders)
        {
            // hitCollider에서 Enemy 있는지 확인
            if (hitCollider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                // Enemy 상태 확인
                if (enemy.Condition.IsDead) return NodeState.FAILURE;

                // 반경 내에 있는 몬스터들 중에서 가장 가까운 몬스터 찾기
                float distanceSqr = (enemy.transform.position - transform.position).sqrMagnitude;

                if (distanceSqr < shortestDistanceSqr)
                {
                    shortestDistanceSqr = distanceSqr;
                    _target = enemy;
                }
            }
        }

        if(_target != null)
        {
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }

    private NodeState IsCanAttack()
    {
        return (_isCanAttack) ? NodeState.SUCCESS : NodeState.FAILURE; 
    }

    private NodeState AttackEnemy()
    {
        if (_target == null) { ("target이 존재 X").EditorLog(); return NodeState.FAILURE; }

        Vector3 dir = (_target.transform.position - transform.position).normalized;
        _advisor.ProjectileManager.ShootProjectile(_advisor, transform.position, dir);
        _isCanAttack = false;

        return NodeState.SUCCESS;
    }

    private void Update()
    {
        _tree.RunTree();
        UpdateAttackCoolTime();
    }

    private void UpdateAttackCoolTime()
    {
        if (_isCanAttack) return;

        _currentAttackCoolTime += Time.deltaTime;
        if(_currentAttackCoolTime > _advisor.Data.AttackInfo.AttackCoolTime)
        {
            _currentAttackCoolTime = 0f;
            _isCanAttack = true;
        }
    }
}
