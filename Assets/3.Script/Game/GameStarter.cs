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
        StartCoroutine(CheckTouch());
    }

    private IEnumerator CheckTouch()
    {
        while (true)
        {
            yield return null;
            moveInput = moveAction.action.ReadValue<Vector2>();

            // 조이스틱 입력이 있을 때만 이동
            if (moveInput.sqrMagnitude > 0.01f)
            {
                StartGame();
                yield break;
            }
        }
    }

    private void StartGame()
    {
        GameManager.Instance.startGameAction?.Invoke();
        Destroy(gameObject);
    }
}
