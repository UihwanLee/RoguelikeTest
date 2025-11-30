using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum ConditionType
{
    Atk,
    Exp,
    MaxExp,
    Gold,
    Hp,
    MaxHp,
    Speed,
    Lv,
    KnockbackPower,
}

public class PlayerCondition : MonoBehaviour
{
    private Condition _maxExp;
    private Condition _maxHp;

    private Condition _atk;
    private Condition _exp;
    private Condition _gold;
    private Condition _hp;
    private Condition _lv;
    private Condition _speed;

    private Transform _damageTransform;  // 데미지를 표시할 Transform

    [field: SerializeField] public ConditionInfoSO _conditionInfo { get; private set; }

    public bool IsDead { get; set; }
    public bool IsInvincibility { get; set; }

    private Player _player;
    private Coroutine _damageFlashCoroutine;

    protected virtual void Awake()
    {
        _player = GetComponent<Player>();

        _atk = new Condition(_conditionInfo.BaseAtk);
        _maxExp = new Condition(_conditionInfo.BaseMaxExp);
        _maxHp = new Condition(_conditionInfo.BaseMaxHp);
        _speed = new Condition(_conditionInfo.BaseSpeed);

        _exp = new Condition(0f);
        _hp = new Condition(_maxHp.Value);
        _lv = new Condition(1f);
        _gold = new Condition(0f);

        if (_player.PlayerConditionUI != null)
        {
            _player.PlayerConditionUI.UpdateExpBar(_exp.Value);
            _player.PlayerConditionUI.UpdateHpBar(_hp.Value / _maxExp.Value);
            _player.PlayerConditionUI.UpdateGold(_gold.ToString());
        }
    }

    protected virtual void Start()
    {
        IsDead = false;
        //_floatingTextPoolManager = FloatingTextPoolManager.Instance;
    }

    public void SetDamageTransform(Transform target)
    {
        // 데미지 표시 위치 초기화
        this._damageTransform = target;
    }

    public void AddCondition(ConditionType tpye, float amount)
    {
        switch (tpye)
        {
            case ConditionType.Atk:
                _atk.AddValue(amount);
                break;
            case ConditionType.Exp:
                _exp.AddValue(amount);
                {
                    if (_exp.Value >= _maxExp.Value)
                    {
                        // 경험치 통 늘리기
                        _maxExp.SetValue(_maxExp.Value * _conditionInfo.ExpIncreaseScaling);

                        // 경험치 초기화
                        _exp.SetValue(0f);

                        // 기본 공격력 증가
                        _atk.SetValue(_atk.Value * _conditionInfo.AtkIncreasScaling);

                        // 레벨 올리기
                        _lv.AddValue(1f);
                    }

                    // UI 전달
                    string lv = _lv.Value.ToString("F0");
                    //_conditionUI.UpdateLevel(lv);
                    if (_player.PlayerConditionUI != null) _player.PlayerConditionUI.UpdateExpBar(_exp.Value / _maxExp.Value);
                }
                break;
            case ConditionType.Gold:
                _gold.AddValue(amount);
                // UI 전달
                string gold = _gold.Value.ToString("F0");
                if (_player.PlayerConditionUI != null) _player.PlayerConditionUI.UpdateGold(gold);
                break;
            case ConditionType.Hp:
                _hp.SetValue(Mathf.Min(_hp.Value + amount, _maxHp.Value));
                if (_player.PlayerConditionUI != null) _player.PlayerConditionUI.UpdateHpBar(_hp.Value / _maxHp.Value);
                break;
            case ConditionType.Speed:
                _speed.AddValue(amount);
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
            case ConditionType.Exp:
                _exp.SubVale(amount);
                break;
            case ConditionType.MaxExp:
                _maxExp.SubVale(amount);
                break;
            case ConditionType.Gold:
                _gold.SubVale(amount);
                string gold = _gold.Value.ToString("F0");
                if (_player.PlayerConditionUI != null) _player.PlayerConditionUI.UpdateGold(gold);
                break;
            case ConditionType.Hp:
                _hp.SetValue(Mathf.Max(0f, _hp.Value - amount));
                if (_player.PlayerConditionUI != null) _player.PlayerConditionUI.UpdateHpBar(_hp.Value / _maxHp.Value);
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
            case ConditionType.Exp:
                _exp.SetValue(amount);
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

    public void TakeDamage(Transform other, float damage, Color? color = null)
    {
        if (IsInvincibility) return;

        // Damage Text 표시
        _player.FloatingTextPoolManager.SpawnText(TextType.Damage, damage.ToString(), this.transform, color);

        // 데미지 효과 적용
        StartDamageFlashCoroutine();

        // 무적 시간 적용 && 일시적 속도 증가
        IsInvincibility = true;
        _speed.AddValue(_speed.Value * _conditionInfo.InvincibilitySpeedMulitplier);

        // damage 적용
        SubCondition(ConditionType.Hp, damage);
    }

    private void StartDamageFlashCoroutine()
    {
        if (_damageFlashCoroutine != null) StopCoroutine(_damageFlashCoroutine);
        _damageFlashCoroutine = StartCoroutine(DamageFlashCoroutine(_conditionInfo.InvincibilityDuration, _conditionInfo.FlashDamageCount));
    }

    private IEnumerator DamageFlashCoroutine(float duration, int count)
    {
        SpriteRenderer spriteRenderer = _player.SpriteRenderer;
        Color originalColor = spriteRenderer.color;
        Color flashColor = Color.red;

        float interval = duration / (2f * count); 

        for (int i = 0; i < count; i++)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(interval);

            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(interval);
        }

        spriteRenderer.color = originalColor;

        // 무적 시간 적용 && 일시적 속도 취소
        IsInvincibility = false;
        _speed.SubVale(_speed.Value * _conditionInfo.InvincibilitySpeedMulitplier);
    }

    public virtual void OnDie()
    {
        // GameOver UI 출력
    }

    public float Atk { get { return _atk.Value; } }
    public float Hp { get { return _hp.Value; } }
    public float MaxHp { get { return _maxHp.Value; } }
    public float Speed { get { return _speed.Value; } }
    public ConditionInfoSO ConditionInfoSO { get { return _conditionInfo; } }
}

