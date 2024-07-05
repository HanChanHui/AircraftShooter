using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    [SerializeField] float rotationSpeed = 700.0f;
    [SerializeField] float gravity = 9.81f;
    [SerializeField] float smoothBlend = 0.1f;


    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private Animator anim;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        RotatePlayerToMouse();
        MovePlayer();
    }

   void RotatePlayerToMouse()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (playerPlane.Raycast(ray, out float hitDist))
        {
            Vector3 targetPoint = ray.GetPoint(hitDist);
            Vector3 direction = targetPoint - transform.position;
            direction.y = 0; // Y축 회전을 방지하여 플레이어가 바닥을 보지 않도록 합니다.
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    void MovePlayer()
    {
        if (characterController.isGrounded)
        {
            // 입력 받기
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // 이동 방향 설정
            Vector3 inputDirection = new Vector3(horizontal, 0, vertical);
            inputDirection = Vector3.ClampMagnitude(inputDirection, 1);

            // 카메라 기준으로 변환
            Vector3 transformedDirection = transform.TransformDirection(inputDirection);

            if (inputDirection.magnitude > 0.1f)
            {
                // 이동 방향 설정
                moveDirection = transformedDirection * speed;

                // 애니메이터 파라미터 설정
                anim.SetFloat("x", inputDirection.x, smoothBlend, Time.deltaTime);
                anim.SetFloat("y", inputDirection.z, smoothBlend, Time.deltaTime);
            }
            // else
            // {
            //     // 멈추기
            //     moveDirection = Vector3.zero;
            //     anim.SetFloat("x", 0);
            //     anim.SetFloat("y", 0);
            // }
        }

        // 중력 적용
        moveDirection.y -= gravity * Time.deltaTime;

        // 이동 적용
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
