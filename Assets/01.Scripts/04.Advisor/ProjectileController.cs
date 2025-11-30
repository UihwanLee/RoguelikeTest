using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private LayerMask _target;
    private Vector3 _fireDirection;
    private CircleCollider2D _circleCollider2D;
    private SpriteRenderer _spriteRenderer;
    private ProjectileManager _manager;
    private Advisor _advisor;

    private void Awake()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        Fire();
        //CheckHit();
        CheckOutOfRange();
    }

    public void Initialize(Advisor advisor, Vector3 position, Vector3 dir, ProjectileManager manager)
    {
        _spriteRenderer.sprite = advisor.Data.ProjectileInfo.ProjectTileSprite;
        _target = advisor.Data.AttackInfo.AttackTarget;
        _fireDirection = dir;
        _manager = manager;

        _advisor = advisor;

        // Position 설정
        transform.position = position;

        // Rotation 설정
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Fire()
    {
        if(_advisor)
            transform.position += _fireDirection * _advisor.Data.ProjectileInfo.ProjectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_target.value == (_target.value | (1 << collision.gameObject.layer)))
        {
            if (collision.TryGetComponent<Enemy>(out Enemy enemy))
            {
                // Enemy에게 데미지
                ("Enemy 타격").EditorLog();
                enemy.Condition.TakeDamage(this.gameObject.transform, _advisor.Condition.KnockbackPower, _advisor.Condition.GetDamage());

                // PoolManager에게 반납
                ReleaseObject();
            }
        }
    }

    private void CheckHit()
    {
        RaycastHit2D hit = (Physics2D.CircleCast(transform.position, _circleCollider2D.radius, _fireDirection, _target));

        if(hit)
        {
            if (hit.collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                // Enemy에게 데미지
                ("Enemy 타격").EditorLog();
                enemy.Condition.TakeDamage(this.gameObject.transform, _advisor.Condition.KnockbackPower, _advisor.Condition.GetDamage());

                // PoolManager에게 반납
                ReleaseObject();
            }
        }
    }

    private void CheckOutOfRange()
    {
        // 영역 밖으로 나갔는지 확인
        if(Mathf.Abs(this.transform.position.x) > 20.0f || Mathf.Abs(this.transform.position.y) > 20.0f)
        {
            ReleaseObject();
        }
    }

    private void ReleaseObject()
    {
        // PoolManager에게 반납
        Debug.Log("반납");
        _manager.ReleaseProjectile(this.gameObject);
    }
}
