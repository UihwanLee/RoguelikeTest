using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class AdvisorCondition : MonoBehaviour
{
    private Condition _maxExp;          // 최대 경험치
    private Condition _maxHp;           // 최대 체력

    private Condition _atk;             // 공격력
    private Condition _exp;             // 경험치
    private Condition _hp;              // 체력
    private Condition _lv;              // 레벨
    private Condition _speed;           // 속도
    private Condition _knockbackPower;  // 넉백 파워

    private Transform _damageTransform;  // 데미지를 표시할 Transform

    [field: SerializeField] public ConditionInfoSO _conditionInfo { get; private set; }

    public bool IsDead { get; set; }

    protected virtual void Awake()
    {
        _atk = new Condition(_conditionInfo.BaseAtk);
        _maxExp = new Condition(_conditionInfo.BaseMaxExp);
        _maxHp = new Condition(_conditionInfo.BaseMaxHp);
        _speed = new Condition(_conditionInfo.BaseSpeed);
        _knockbackPower = new Condition(_conditionInfo.KnockbackPower);

        _exp = new Condition(0f);
        _hp = new Condition(_maxHp.Value);
        _lv = new Condition(1f);
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
                    //_conditionUI.UpdateExpBar(_exp.Value / _maxExp.Value);
                }
                break;
            case ConditionType.Hp:
                _hp.SetValue(Mathf.Min(_hp.Value + amount, _maxHp.Value));
                break;
            case ConditionType.Speed:
                _speed.AddValue(amount);
                break;
            case ConditionType.KnockbackPower:
                _knockbackPower.AddValue(amount);
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
            case ConditionType.Hp:
                _hp.SetValue(Mathf.Max(0f, _hp.Value - amount));
                {
                    Transform damageTransform = this.transform;

                    // Damage Text 표시
                    //_floatingTextPoolManager.SpawnText(TextType.Damage, amount.ToString(), damageTransform, color);

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
            case ConditionType.KnockbackPower:
                _knockbackPower.SubVale(amount);
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
            case ConditionType.Hp:
                _hp.SetValue(amount);
                break;
            case ConditionType.MaxHp:
                _maxHp.SetValue(amount);
                break;
            case ConditionType.Speed:
                _speed.SetValue(amount);
                break;
            case ConditionType.KnockbackPower:
                _knockbackPower.SetValue(amount);
                break;
            default:
                break;
        }
    }

    public virtual void OnDie()
    {

    }

    /// <summary>
    /// 데미지 계산 공식으로 인한 데미지 구하기
    /// </summary>
    /// <returns></returns>
    public float GetDamage()
    {
        return Atk;
    }

    public float Atk { get { return _atk.Value; } }
    public float Hp { get { return _hp.Value; } }
    public float MaxHp { get { return _maxHp.Value; } }
    public float Speed { get { return _speed.Value; } }
    public float KnockbackPower { get { return _knockbackPower.Value; } } 
    public ConditionInfoSO ConditionInfoSO { get { return _conditionInfo; } }
}

