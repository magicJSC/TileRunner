using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GameOver : MonoBehaviour
{
    private GameObject newScorePanel;
    private GameObject endPanel;
    private GameObject beforeBestTitle;

    private TextMeshProUGUI bestScoreText;
    private TextMeshProUGUI beforeBestScoreText;

    private void Start()
    {
        newScorePanel = Util.FindChild(gameObject, "NewScorePanel");
        endPanel = Util.FindChild(gameObject, "EndPanel");
        beforeBestTitle = Util.FindChild(gameObject, "BestScoreTitle");

        bestScoreText = Util.FindChild<TextMeshProUGUI>(gameObject, "Score");
        beforeBestScoreText = Util.FindChild<TextMeshProUGUI>(gameObject, "BeforeScore");

        if (CheckBestScore())
        {
            newScorePanel.SetActive(true);
            endPanel.SetActive(false);

            bestScoreText.text = $"{GameManager.Instance.Score}";
            GameManager.Instance.bestScore = GameManager.Instance.Score;
            beforeBestTitle.SetActive(false);
        }
        else
        {
            CloseBestScorePanel();
            beforeBestScoreText.text = $"{GameManager.Instance.bestScore}";
        }
    }


    private bool CheckBestScore()
    {
        return GameManager.Instance.Score > GameManager.Instance.bestScore;
    }

    /// <summary>
    /// 최고 점수 패널 닫고 종료 패널 열기
    /// </summary>
    public void CloseBestScorePanel()
    {
        newScorePanel.SetActive(false);
        endPanel.SetActive(true);
    }

    /// <summary>
    /// 게임 재시작
    /// </summary>
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
