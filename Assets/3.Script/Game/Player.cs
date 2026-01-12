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

    [Header("Jump")]
    [SerializeField] private float jumpCooldown = 0.5f;
    [SerializeField] Transform jumpCheckPos;

    private void Awake()
    {
        GameManager.Instance.player = transform;
        isGrounded = true;
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        if (GameManager.Instance != null)
            GameManager.Instance.startGameAction -= StartGame;
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        GameManager.Instance.startGameAction += StartGame;
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

    void RotateByJoystick(Vector2 input)
    {
        float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.Euler(0, targetAngle, 0),
            turnSpeed * Time.deltaTime
        );
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
            var cols = Physics.OverlapSphere(jumpCheckPos.position, 0.3f);
            foreach(var col in cols)
            {
                if (col.GetComponent<HexTile>())
                {
                    canJump = false;
                    break;
                }
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
