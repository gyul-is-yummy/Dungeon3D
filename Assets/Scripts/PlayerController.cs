using System.Collections;
using System.Collections.Generic;
using System.Net;
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
    public float moveSpeed = 5;                     //이동속도
    public float jumpPower = 10;                    //점프력
    [SerializeField] private int jumpCount = 0;     //현재 플레이어가 몇 번째 점프중인지
    public int additionalJump = 1;                  //추가 점프 설정 (1로 설정시, 2단 점프까지 가능)
    public PlayerState state = PlayerState.Idle;    //현재 플레이어의 상태
    private Vector2 movementInput;                  //플레이어의 이동 입력값

    [Header("Check the private")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask groundLayerMask;


    private void OnValidate()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        groundLayerMask = ~LayerMask.GetMask("Player");
    }

    private void FixedUpdate()
    {
        Move();
        _animator.SetBool("IsMoving", state != PlayerState.Idle);

        //switch (state)
        //{
        //    case PlayerState.Idle:
                
        //        break;
        //    case PlayerState.Move:
        //        break;
        //    case PlayerState.Jump:
        //        break;
        //    case PlayerState.Attack:
        //        break;
        //}
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //키가 눌렸을 때
        if (context.phase == InputActionPhase.Performed)
        {
            //Debug.Log($"이동 키 눌림");
            movementInput = context.ReadValue<Vector2>();
            state = PlayerState.Move;
        }
        //키가 떨어졌을 때
        else if (context.phase == InputActionPhase.Canceled)
        {
            //Debug.Log($"이동 키 떨어짐");
            movementInput = Vector2.zero;
            state = PlayerState.Idle;
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * movementInput.y + transform.right * movementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;  //이걸 쓰지 않으면 y축이 0으로 고정된다. (안 쓰면 점프 불가)

        _rigidbody.velocity = dir;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //땅에 있다면, 점프 카운트를 초기화
        //업데이트에 넣으면 불필요한 검사가 너무 많이 일어날 것 같아서 여기 넣어줌
        if (IsGrounded())
        {
            jumpCount = 0;
        }

        if (context.phase == InputActionPhase.Started)
        {
            
            //한계 횟수 이상으로 뛰려고 하면 더이상 뛰지 못하도록 돌려보내준다.
            if (jumpCount >= additionalJump) return;

            if(IsGrounded())
            {
                _animator.SetTrigger("Jump");
            }    
            else
            {
                _animator.SetTrigger("DoubleJump");
            }

            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            jumpCount++;
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
       {
            //책상 다리를 만들어준다.
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
       };

        for (int i = 0; i < rays.Length; i++)
        {
            //0.1f 길이의 ray를 쏜다.
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                //하나라도 닿으면 true
                return true;
            }
        }

        //하나도 닿지 않으면 false
        return false;
    }
}
