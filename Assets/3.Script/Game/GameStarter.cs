using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction; // Vector2 (조이스틱)
    private Vector2 moveInput;
    private void Start()
    {
        GameManager.Instance.Score = 0;
        GameManager.Instance.isGameOver = false;

        GameManager.Instance.isStart = false;

        GameManager.Instance.restartAction += ReStartAction;

        StartCoroutine(CheckTouch());
    }

    private IEnumerator CheckTouch()
    {
        while (true)
        {
            yield return null;

            moveInput = moveAction.action.ReadValue<Vector2>();

            if (GameManager.Instance.isStart)
                yield break;
            // 조이스틱 입력이 있을 때만 이동
            if (moveInput.sqrMagnitude > 0.01f)
            {
                GameManager.Instance.isStart = true;
                StartGame();
                yield break;
            }
        }
    }

    /// <summary>
    /// 골에서 멈추고 재시작
    /// </summary>
    private void ReStartAction()
    {
        Debug.Log("리스타트");
        StartCoroutine(CheckTouch());
    }

    private void StartGame()
    {
        GameManager.Instance.startGameAction?.Invoke();
    }
}
