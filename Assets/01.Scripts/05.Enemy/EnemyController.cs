using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Enemy _enemy;
    private BehaviorTree _tree;
    private Transform _target;
    private bool _isCanAttack = false;
    private float _currentAttackCoolTime = 0f;

    private void Awake()
    {
        
    }

    public void Init(Enemy enemy)
    {
        _enemy = enemy;

        SetBehavior();

        // Target 지정
        _target = GameManager.Instance.Player.transform;
    }

    private void SetBehavior()
    {
        // 리프 노드 생성
        ActionNode findPlayer = new ActionNode("Find Target with Layer", FindTarget);
        ActionNode chaseToTarget = new ActionNode("Chase Target", ChaseToTarget);
        ConditionNode isCanAttack = new ConditionNode("Is Can Attack Player?", IsCanAttack);
        ActionNode attackPlayer = new ActionNode("Attack Player", AttackPlayer);

        // 복합 노드 생성
        SequenceNode root = new SequenceNode("find and chase Target", findPlayer, chaseToTarget, isCanAttack, attackPlayer);

        // BehaviorTree 생성
        _tree = new BehaviorTree(root);
    }

    private NodeState FindTarget()
    {
        // Target 상태 확인 : 존재, 죽음
        return (_target != null) ? NodeState.SUCCESS : NodeState.FAILURE;  
    }

    private NodeState ChaseToTarget()
    {
        // 타켓 위치로 이동 : AttackRange까지 접근
        if(Vector3.Distance(transform.position, _target.position) < _enemy.Data.AttackInfo.AttackRange)
        {
            return NodeState.SUCCESS;
        }

        // Target 위치로 Slerp 
        transform.position = Vector3.Lerp(transform.position, _target.position, _enemy.Data.MoveSpeed * Time.deltaTime);

        // Target과의 거리에 따른 Flip
        _enemy.SpriteRenderer.flipX = (transform.position.x - _target.position.x) > 0;

        return NodeState.RUNNING;
    }

    private NodeState IsCanAttack()
    {
        return (_isCanAttack) ? NodeState.SUCCESS : NodeState.FAILURE;
    }

    private NodeState AttackPlayer()
    {
        if (_target == null) { ("target이 존재 X").EditorLog(); return NodeState.FAILURE; }

        // 플레이어 공격
        Player player = _target.GetComponent<Player>();
        player.Condition.TakeDamage(this.transform, _enemy.Condition.Atk, Color.red);
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
        if (_currentAttackCoolTime > _enemy.Data.AttackInfo.AttackCoolTime)
        {
            _currentAttackCoolTime = 0f;
            _isCanAttack = true;
        }
    }
}
