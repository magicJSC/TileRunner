using UnityEngine;

public class GameEnder : MonoBehaviour
{
    public static GameEnder Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] GameObject gameOverUI;

    public void EndGame()
    {
        if (GameManager.Instance.isGameOver) return;

        GameManager.Instance.isGameOver = true;
        GameManager.Instance.isIncreaseDoubleScore = false;
        Instantiate(gameOverUI);
    }
}
