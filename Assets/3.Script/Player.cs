using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class Player : MonoBehaviour
{
    public float turnSpeed = 150;
    public float moveSpeed = 5f;
    public float jumpHeight = 10f;

    private bool leftPressed;
    private bool rightPressed;

    private Vector3 velocity;

    [SerializeField] private float gravity = -9.81f;
    private CharacterController controller;

    private bool isGrounded;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        StartCoroutine(CheckInputCor());
        StartCoroutine(SetVelocityToCharacter());
    }

    /// <summary>
    /// 모바일 터치 혹은 마우스 좌,우 클릭을 받아 행동
    /// </summary>
    IEnumerator CheckInputCor()
    {
        while (true)
        {
            yield return null;

            leftPressed = false;
            rightPressed = false;

            if (Touchscreen.current != null)
            {
                HandleTouch();
            }
            else
            {
                HandleMouse();
            }

            // 양쪽 누르면 후진
            if (leftPressed && rightPressed && isGrounded)
            {
                Jump();
                continue;
            }

            // 좌/우 각각 조향
            if (leftPressed)
                TurnLeft();

            if (rightPressed)
                TurnRight();

            Forward();
        }
    }

    /// <summary>
    /// 캐릭터 중력 적용
    /// </summary>
    private IEnumerator SetVelocityToCharacter()
    {
        while (true)
        {
            yield return null;
            //중력
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            isGrounded = controller.isGrounded;
            if (isGrounded && velocity.y < 0)
                velocity.y = -2f;
        }
    }

    /// <summary>
    /// 모바일 터치 왼쪽과 오른쪽에 터치 확인
    /// </summary>
    void HandleTouch()
    {
        foreach (var t in Touchscreen.current.touches)
        {
            if (!t.press.isPressed) continue;

            Vector2 pos = t.position.ReadValue();
            if (pos.x < Screen.width * 0.5f)
                leftPressed = true;
            else
                rightPressed = true;
        }
    }

    /// <summary>
    /// 마우스 좌, 우클릭 확인
    /// </summary>
    void HandleMouse()
    {
        if (Mouse.current.leftButton.isPressed)
            leftPressed = true;

        if (Mouse.current.rightButton.isPressed)
            rightPressed = true;
    }

    void TurnLeft()
    {
        transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
    }

    void TurnRight()
    {
        transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
    }

    void Forward()
    {
        controller.Move(transform.forward * moveSpeed * Time.deltaTime);
    }

    void Jump()
    {
        if (!isGrounded)
            return;
        float jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        velocity.y = jumpVelocity;
        isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        HexTile tile = other.GetComponent<HexTile>();
        if (tile != null)
        {
            isGrounded = true;
            tile.OnStepped();
        }
    }
}
