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

        if (GameManager.Instance.showAds)
        {
            AdsManager.Instance.ShowAd();
            GameManager.Instance.showAds = false;
        }
        GameManager.Instance.isGameOver = true;
        GameManager.Instance.endGameAction?.Invoke();
        Instantiate(gameOverUI);
    }
}
