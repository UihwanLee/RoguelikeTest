using UnityEngine;

public class Condition
{
    [SerializeField] private float _value;

    public Condition(float initValue)
    {
        _value = initValue;
    }

    public void AddValue(float amount)
    {
        this._value += amount;
    }

    public void SubVale(float amount)
    {
        this._value -= amount;
    }

    public void SetValue(float value)
    {
        this._value = value;
    }

    public float Value { get { return _value; } }
}
