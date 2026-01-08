using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

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
    private Animator anim;

    private bool isGrounded;
    private bool isJump;

    [Header("Slam Skill")]
    [SerializeField] private float slamSpeed = 25f;
    [SerializeField] private float slamDelay = 0.2f;
    private float jumpTimestamp;
    public bool isSlaming { get; private set; }
    private bool canSlam;

    [Header("Jump Cooldown")]
    [SerializeField] private float jumpCooldown = 0.5f; // 착지 후 점프 불가 시간
    private float lastGroundedTime; // 마지막으로 땅에 닿은 시점

    private void Awake()
    {
        GameManager.Instance.player = transform;
        canSlam = false;
        isGrounded = true;
        isSlaming = false;
        lastGroundedTime = -jumpCooldown; // 시작하자마자 바로 점프 가능하게 초기화
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        GameManager.Instance.startGameAction += StartGame;


    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.startGameAction -= StartGame;
    }

    private void StartGame()
    {
        StartCoroutine(CheckInputCor());
        StartCoroutine(SetVelocityToCharacter());
        anim.SetTrigger("run");
    }

    IEnumerator CheckInputCor()
    {
        while (true)
        {
            yield return null;

            if (GameManager.Instance.isGameOver)
                yield break;

            leftPressed = false;
            rightPressed = false;

            if (Touchscreen.current != null)
                HandleTouch();
            else
                HandleMouse();

            Forward();

            if (leftPressed && rightPressed)
            {
                // 수정된 부분: 땅에 있고 + 점프 중이 아니며 + 착지 후 쿨타임이 지났을 때만 점프
                if (isGrounded && !isJump && (Time.time - lastGroundedTime > jumpCooldown))
                {
                    Jump();
                }
                else if (canSlam && !isSlaming && (Time.time - jumpTimestamp > slamDelay))
                {
                    StartSlam();
                }
                continue;
            }

            if (leftPressed) TurnLeft();
            else if (rightPressed) TurnRight();
        }
    }

    private IEnumerator SetVelocityToCharacter()
    {
        while (true)
        {
            yield return null;

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

            bool wasGrounded = isGrounded;
            isGrounded = controller.isGrounded;

            // 공중에 있다가 방금 막 땅에 닿았을 때 (착지 순간)
            if (!wasGrounded && isGrounded && velocity.y < 0)
            {
                lastGroundedTime = Time.time; // 착지 시점 기록
                velocity.y = -2f;
                isJump = false;
                anim.SetBool("keepJump", false);
            }
            // 이미 땅에 있는 상태 유지 시
            else if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
        }
    }

    void StartSlam()
    {
        canSlam = false;
        isSlaming = true;
        velocity.y = -slamSpeed;
    }

    void HandleTouch()
    {
        foreach (var t in Touchscreen.current.touches)
        {
            if (!t.press.isPressed) continue;
            Vector2 pos = t.position.ReadValue();
            if (pos.x < Screen.width * 0.5f) leftPressed = true;
            else rightPressed = true;
        }
    }

    void HandleMouse()
    {
        if (Mouse.current.leftButton.isPressed) leftPressed = true;
        if (Mouse.current.rightButton.isPressed) rightPressed = true;
    }

    void TurnLeft() => transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
    void TurnRight() => transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
    void Forward() => controller.Move(transform.forward * moveSpeed * Time.deltaTime);

    void Jump()
    {
        float jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        velocity.y = jumpVelocity;

        isGrounded = false;
        canSlam = true;
        isJump = true;
        jumpTimestamp = Time.time;

        anim.SetTrigger("jump");
        anim.SetBool("keepJump", true);
    }

    public void Die()
    {
        GameEnder.Instance.EndGame();
        anim.Play("Die");
    }

    public void Fall()
    {
        GameEnder.Instance.EndGame();
        anim.SetTrigger("fall");
    }

    private void OnTriggerEnter(Collider other)
    {
        HexTile tile = other.GetComponent<HexTile>();
        if (tile != null)
        {
            if (isJump && velocity.y < 0)
            {
                isJump = false;
                canSlam = false;
                anim.SetBool("keepJump", false);
            }

            if (isSlaming)
            {
                tile.OnSlamStepped();
                isSlaming = false;
            }
            else
            {
                tile.OnStepped();
            }
        }
    }
}