using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    Idle,
    Move,
    Jump,
    Attack
}

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5;
    public float jumpPower = 80;
    public PlayerState state = PlayerState.Idle;
    private Vector2 movementInput;

    [Header("Check the Connection")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;


    private void OnValidate()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        _animator.SetBool("IsMoving", state != PlayerState.Idle);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //Ű�� ������ ��
        if (context.phase == InputActionPhase.Performed)
        {
            //Debug.Log($"�̵� Ű ����");
            movementInput = context.ReadValue<Vector2>();
            state = PlayerState.Move;
        }
        //Ű�� �������� ��
        else if (context.phase == InputActionPhase.Canceled)
        {
            //Debug.Log($"�̵� Ű ������");
            movementInput = Vector2.zero;
            state = PlayerState.Idle;
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * movementInput.y + transform.right * movementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;  //�̰� ���� ������ y���� 0���� �����ȴ�.

        _rigidbody.velocity = dir;
    }

}
