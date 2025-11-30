using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemyCondition : MonoBehaviour
{
    private Condition _maxExp;
    private Condition _maxHp;

    private Condition _atk;
    private Condition _gold;
    private Condition _hp;
    private Condition _lv;

    private Condition _speed;

    public bool IsDead { get; set; }

    [field: SerializeField] public ConditionInfoSO _conditionInfo { get; private set; }

    private Enemy _enemy;
    private Coroutine _damageFlashCoroutine;

    protected virtual void Awake()
    {
        _enemy = GetComponent<Enemy>();

        _atk = new Condition(_conditionInfo.BaseAtk);
        _maxExp = new Condition(_conditionInfo.BaseMaxExp);
        _maxHp = new Condition(_conditionInfo.BaseMaxHp);
        _speed = new Condition(_conditionInfo.BaseSpeed);

        _hp = new Condition(_maxHp.Value);
        _lv = new Condition(1f);
        _gold = new Condition(0f);
    }

    protected virtual void Start()
    {
        IsDead = false;
    }

    public void AddCondition(ConditionType tpye, float amount)
    {
        switch (tpye)
        {
            case ConditionType.Atk:
                _atk.AddValue(amount);
                break;
            case ConditionType.Gold:
                _gold.AddValue(amount);
                break;
            case ConditionType.Hp:
                _hp.SetValue(Mathf.Min(_hp.Value + amount, _maxHp.Value));
                break;
            case ConditionType.Speed:
                _speed.AddValue(amount);
                break;
            case ConditionType.Lv:
                _lv.AddValue(amount);
                {
                    // 체력 늘리기

                    // 공격력 늘리기
                }
                break;
            default:
                break;
        }
    }

    public void SubCondition(ConditionType tpye, float amount, Color? color = null)
    {
        switch (tpye)
        {
            case ConditionType.Atk:
                _atk.SubVale(amount);
                break;
            case ConditionType.MaxExp:
                _maxExp.SubVale(amount);
                break;
            case ConditionType.Gold:
                _gold.SubVale(amount);
                break;
            case ConditionType.Hp:
                _hp.SetValue(Mathf.Max(0f, _hp.Value - amount));
                {
                    Transform damageTransform = this.transform;

                    if (_hp.Value == 0)
                    {
                        IsDead = true;
                        OnDie();
                    }
                }
                break;
            case ConditionType.MaxHp:
                _maxHp.SubVale(amount);
                break;
            case ConditionType.Speed:
                _speed.SubVale(amount);
                break;
            default:
                break;
        }
    }

    public void SetCondition(ConditionType tpye, float amount)
    {
        switch (tpye)
        {
            case ConditionType.Atk:
                _atk.SetValue(amount);
                break;
            case ConditionType.MaxExp:
                _maxExp.SetValue(amount);
                break;
            case ConditionType.Gold:
                _gold.SetValue(amount);
                break;
            case ConditionType.Hp:
                _hp.SetValue(amount);
                break;
            case ConditionType.MaxHp:
                _maxHp.SetValue(amount);
                break;
            case ConditionType.Speed:
                _speed.SetValue(amount);
                break;
            default:
                break;
        }
    }

    public void TakeDamage(Transform other, float knockbackPower, float damage, Color? color = null)
    {
        // Damage Text 표시
        _enemy.FloatingTextPoolManager.SpawnText(TextType.Damage, damage.ToString(), this.transform, color);

        // 데미지 효과 적용
        StartDamageFlashCoroutine();

        // Knockback 적용
        Vector3 knockbackDirection = (transform.position - other.position).normalized;
        transform.position += knockbackDirection * knockbackPower;

        // damage 적용
        SubCondition(ConditionType.Hp, damage);
    }

    private void StartDamageFlashCoroutine()
    {
        if (_damageFlashCoroutine != null) StopCoroutine(_damageFlashCoroutine);
        _damageFlashCoroutine = StartCoroutine(DamageFlashCoroutine(0.5f, 1));
    }

    private IEnumerator DamageFlashCoroutine(float duration, int count)
    {
        SpriteRenderer spriteRenderer = _enemy.SpriteRenderer;
        Color originalColor = spriteRenderer.color;
        Color flashColor = Color.red;

        float interval = duration / (2f * count); // 켜짐/꺼짐 횟수를 고려한 간격

        for (int i = 0; i < count; i++)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(interval);

            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(interval);
        }

        spriteRenderer.color = originalColor;
    }

    public virtual void OnDie()
    {
        // 반납
        _enemy.SpawnManager.Release(_enemy.Data.Name, this.gameObject);
    }

    public float Atk { get { return _atk.Value; } }
    public float Hp { get { return _hp.Value; } }
    public float MaxHp { get { return _maxHp.Value; } }
    public float Speed { get { return _speed.Value; } }
    public ConditionInfoSO ConditionInfoSO { get { return _conditionInfo; } }
}
