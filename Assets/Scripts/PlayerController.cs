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
    public float moveSpeed = 5;                     //�̵��ӵ�
    public float jumpPower = 10;                    //������
    [SerializeField] private int jumpCount = 0;     //���� �÷��̾ �� ��° ����������
    public int additionalJump = 1;                  //�߰� ���� ���� (1�� ������, 2�� �������� ����)
    public PlayerState state = PlayerState.Idle;    //���� �÷��̾��� ����
    private Vector2 movementInput;                  //�÷��̾��� �̵� �Է°�

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

    public Action inventory;                //�κ��丮�� ���� ���� ��������Ʈ

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
        dir.y = _rigidbody.velocity.y;  //�̰� ���� ������ y���� 0���� �����ȴ�. (�� ���� ���� �Ұ�)

        _rigidbody.velocity = dir;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //���� �ִٸ�, ���� ī��Ʈ�� �ʱ�ȭ
        //������Ʈ�� ������ ���ʿ��� �˻簡 �ʹ� ���� �Ͼ �� ���Ƽ� ���� �־���
        if (IsGrounded())
        {
            jumpCount = 0;
        }

        if (context.phase == InputActionPhase.Started)
        {
            
            //�Ѱ� Ƚ�� �̻����� �ٷ��� �ϸ� ���̻� ���� ���ϵ��� ���������ش�.
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
            //å�� �ٸ��� ������ش�.
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
       };

        for (int i = 0; i < rays.Length; i++)
        {
            //0.1f ������ ray�� ���.
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                //�ϳ��� ������ true
                return true;
            }
        }

        //�ϳ��� ���� ������ false
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
        //�ΰ����� �����ش�.
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        //cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        //TabŰ�� ������ inventory ��������Ʈ�� �ִ� �Լ��� ȣ��
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    //Ŀ�� ���
    void ToggleCursor()
    {
        //Ŀ���� ����ִٴ� ����, �κ��丮 â�� ������ �ʾҴٴ� ���� �ǹ�
        //���� ���콺 �̵��� ���� ȭ���� �����̰� �ִ� ���̶�� ��
        bool toggle = Cursor.lockState == CursorLockMode.Locked;

        //���� Ŀ���� ����ִٸ� Ŀ���� Ǯ���ְ�, �׷��� �ʴٸ� �ٽ� Ŀ���� ��ٴ�.
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

}
