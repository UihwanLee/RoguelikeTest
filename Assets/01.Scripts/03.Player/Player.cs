using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Player : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer { get; private set; }
    public PlayerController Controller { get; private set; }
    public PlayerCondition Condition { get; private set; }
    public FloatingTextPoolManager FloatingTextPoolManager { get; private set; }
    public PlayerConditionUI PlayerConditionUI { get; private set; }

    private void Awake()
    {
        Controller = GetComponent<PlayerController>();
        Condition = GetComponent<PlayerCondition>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        FloatingTextPoolManager = FloatingTextPoolManager.Instance;
        PlayerConditionUI = GameObject.Find("PlayerConditionUI").GetComponent<PlayerConditionUI>();
    }
}
