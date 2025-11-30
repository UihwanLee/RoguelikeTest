using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerConditionUI : MonoBehaviour
{
    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _expBar;

    private void Awake()
    {
        _hpBar = transform.FindChild<Image>("HpBar");
        _expBar = transform.FindChild<Image>("ExpBar");
    }

    public void UpdateHpBar(float percentage)
    {
        _hpBar.fillAmount = percentage;
    }

    public void UpdateExpBar(float percentage)
    {
        _expBar.fillAmount = percentage;
    }
}
