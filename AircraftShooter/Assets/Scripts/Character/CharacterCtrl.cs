using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour
{
    public enum AnimState
    {
        Idle,
        Walk,
        Run,
    };

    [SerializeField] float speed = 5.0f;
    [SerializeField] float rotationSpeed = 700.0f;
    [SerializeField] float gravity = 9.81f;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private Animator anim;

    private void Awake() 
    {
         characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        AnimationState(AnimState.Idle);
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (characterController.isGrounded)
        {
            // 입력 받기
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // 입력 방향 설정
            Vector3 inputDirection = new Vector3(horizontal, 0, vertical);
            if (inputDirection.magnitude > 0.1f)
            {
                AnimationState(AnimState.Walk);
                // 입력 방향으로 회전
                Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // 이동 방향 설정
                moveDirection = inputDirection.normalized * speed;
            }
            else
            {
                // 멈추기
                AnimationState(AnimState.Idle);
                moveDirection = Vector3.zero;
            }
        }

        // 중력 적용
        moveDirection.y -= gravity * Time.deltaTime;

        // 이동 적용
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void AnimationState(AnimState _state)
    {
        switch(_state)
        {
            case AnimState.Idle:
                anim.SetBool("IsWalk", false);
                break;
            case AnimState.Walk:
                anim.SetBool("IsWalk", true);
                break;
            case AnimState.Run:

                break;
        }
    }
}
