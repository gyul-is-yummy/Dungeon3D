using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

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

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook = -85;
    public float maxXLook = 85;
    private float camCurXRot;
    public float lookSensitivity = 0.1f;
    private Vector2 mouseDelta;
    private Vector2 mouseScrollDelta;
    private bool canLook = true;

    [Header("Check the private")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask groundLayerMask;

    public Action inventory;                //인벤토리를 열기 위한 델리게이트

    private void OnValidate()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        groundLayerMask = ~LayerMask.GetMask("Player");
        cameraContainer = GetComponentInChildren<Camera>().transform;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
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

    private void LateUpdate()
    {
        CameraLook();
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

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
    public void OnPerspective(InputAction.CallbackContext context)
    {
        mouseScrollDelta = context.ReadValue<Vector2>();
    }

    void CameraLook()
    {
        //민감도를 곱해준다.
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        //cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        //Tab키를 누르면 inventory 델리게이트에 있는 함수를 호출
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    //커서 토글
    void ToggleCursor()
    {
        //커서가 잠겨있다는 것은, 인벤토리 창이 열리지 않았다는 것을 의미
        //또한 마우스 이동에 따라 화면이 움직이고 있는 중이라는 뜻
        bool toggle = Cursor.lockState == CursorLockMode.Locked;

        //만약 커서가 잠겨있다면 커서를 풀어주고, 그렇지 않다면 다시 커서를 잠근다.
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

}
