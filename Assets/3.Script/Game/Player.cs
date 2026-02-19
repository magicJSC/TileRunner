using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float turnSpeed = 150f;
    public float moveSpeed = 5f;
    public float jumpHeight = 10f;

    private Vector3 velocity;

    [SerializeField] private float gravity = -9.81f;
    private CharacterController controller;
    private Animator anim;

    private bool isGrounded;

    [Header("Input")]
    [SerializeField] private InputActionReference moveAction; // Vector2 (조이스틱)

    private Vector2 moveInput;

    private Transform camTransform;

    Coroutine moveCor;
    Coroutine jumpCor;

    [Header("Jump")]
    [SerializeField] Transform jumpCheckPos;

    private void Awake()
    {
        GameManager.Instance.Player = transform;
        isGrounded = true;
        camTransform = Camera.main.transform;
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.startGameAction -= StartGame;
            GameManager.Instance.resetAction -= ResetAction;
        }
    }

    private void Start()
    {
        moveAction.action.Enable();

        jumpCheckPos = Util.FindChild<Transform>(gameObject, "CheckPos");

        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        GameManager.Instance.startGameAction += StartGame;
        GameManager.Instance.resetAction += ResetAction;
    }

    private void StartGame()
    {
        moveCor = StartCoroutine(CheckInputCor());
        jumpCor = StartCoroutine(SetVelocityToCharacter());
        anim.SetTrigger("run");
    }

    private void ResetAction()
    {
        anim.Play("Idle");
        StopCoroutine(moveCor);
        StopCoroutine(jumpCor);
    }

    IEnumerator CheckInputCor()
    {
        while (true)
        {
            yield return null;

            if (GameManager.Instance.isGameOver)
                yield break;



            moveInput = moveAction.action.ReadValue<Vector2>();

            // 조이스틱 입력이 있을 때만 이동
            if (moveInput.sqrMagnitude > 0.01f)
            {
                RotateByJoystick(moveInput);
            }

            MoveForward();
            CheckCanJump();
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

            if (!wasGrounded && isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
                anim.SetBool("keepJump", false);
            }
            else if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
        }
    }

    public void RotateByJoystick(Vector2 input)
    {
        // 1. 데드존 처리
        if (input.sqrMagnitude < 0.1f)
            return;

        // 2. 조이스틱 입력을 각도로 변환
        float joystickAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;

        // 3. 메인 카메라의 Y축 회전값 가져오기
        float cameraAngle = camTransform.eulerAngles.y;

        // 4. 조이스틱 각도에 카메라 각도를 더해서 최종 목표 각도 계산
        float targetAngle = joystickAngle + cameraAngle;

        // 5. 즉시 회전 적용
        transform.rotation = Quaternion.Euler(0, targetAngle, 0);
    }


    void MoveForward()
    {
        controller.Move(transform.forward * moveSpeed * Time.deltaTime);
    }

    void CheckCanJump()
    {
        if (isGrounded)
        {
            bool canJump = true;
            var cols = Physics.OverlapSphere(jumpCheckPos.position, 0.4f);
            foreach (var col in cols)
            {
                canJump = false;
                break;
            }

            if (canJump)
                Jump();
        }
    }

    void Jump()
    {
        float jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        velocity.y = jumpVelocity;

        isGrounded = false;

        anim.SetTrigger("jump");
        anim.SetBool("keepJump", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        HexTile tile = other.GetComponent<HexTile>();
        if (tile == null) return;

        if (!GameManager.Instance.isStart || GameManager.Instance.isGameOver)
            return;
        tile.OnStepped();
       if(velocity.y < 0)
        {
            anim.SetBool("keepJump", false);
        }
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
}
