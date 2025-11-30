using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerConditionUI : MonoBehaviour
{
    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _expBar;
    [SerializeField] private TextMeshProUGUI _goldText;

    private void Awake()
    {
        _hpBar = transform.FindChild<Image>("HpBar");
        _expBar = transform.FindChild<Image>("ExpBar");
        _goldText = transform.FindChild<TextMeshProUGUI>("GoldText");
    }

    public void UpdateHpBar(float percentage)
    {
        _hpBar.fillAmount = percentage;
    }

    public void UpdateExpBar(float percentage)
    {
        _expBar.fillAmount = percentage;
    }

    public void UpdateGold(string gold)
    {
        _goldText.text = gold;
    }
}
