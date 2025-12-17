using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStarter : MonoBehaviour
{
    [SerializeField] GameObject scoreUI;
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
            if (Touchscreen.current != null)
            {
                if (Touchscreen.current.touches.Count != 0)
                {
                    StartGame();
                }
            }
            else
            {
                if (Mouse.current.leftButton.isPressed)
                    StartGame();
                if (Mouse.current.rightButton.isPressed)
                    StartGame();
            }
        }
    }

    private void StartGame()
    {
        GameManager.Instance.startGameAction?.Invoke();
        Instantiate(scoreUI);
        Destroy(gameObject);
    }
}
