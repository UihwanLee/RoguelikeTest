using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private float _moveSpeed = 4f;
    private Vector2 _curMoveInput;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Move();
        Flip();
    }

    /// <summary>
    /// 캐릭터 이동
    /// </summary>
    private void Move()
    {
        Vector3 moveDirection = transform.up * _curMoveInput.y + transform.right * _curMoveInput.x;
        Vector3 displacement = moveDirection.normalized * _moveSpeed * Time.fixedDeltaTime;
        transform.position += displacement;
    }

    /// <summary>
    /// 캐릭터 이미지 Flip
    /// </summary>
    private void Flip()
    {
        _spriteRenderer.flipX = (_curMoveInput.x < 0.0f);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _curMoveInput = context.ReadValue<Vector2>();
            //Debug.Log(_curMoveInput);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _curMoveInput = Vector2.zero;
        }
    }
}
