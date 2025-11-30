using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TextType
{
    Damage,
    Gold,
    Exp,
}

[CreateAssetMenu(fileName = "FloatingText", menuName = "UI/FloatingText")]
public class FloatingTextSO : ScriptableObject
{
    public string textName;
    public TextType type;
    public Color color;
    public float duration;
    public float floatingDist;
}
