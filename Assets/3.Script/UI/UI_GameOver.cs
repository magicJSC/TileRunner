using DG.Tweening;
using GooglePlayGames;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour
{
    private GameObject newScorePanel;
    private GameObject endPanel;
    private GameObject beforeBestTitle;

    private TextMeshProUGUI bestScoreText;
    private TextMeshProUGUI beforeBestScoreText;


    [SerializeField] float fillTime;
    [SerializeField] Image fill;

    [SerializeField] GameObject revivePanel;

    [SerializeField] UI_EventHandler reviveEvent;
    [SerializeField] UI_EventHandler cancelEvent;

    [SerializeField] AudioClip clickSound;

    Coroutine fillCor;

    private void Start()
    {
        newScorePanel = Util.FindChild(gameObject, "NewScorePanel");
        endPanel = Util.FindChild(gameObject, "EndPanel");
        beforeBestTitle = Util.FindChild(gameObject, "BestScoreTitle");

        bestScoreText = Util.FindChild<TextMeshProUGUI>(gameObject, "Score");
        beforeBestScoreText = Util.FindChild<TextMeshProUGUI>(gameObject, "BestScore");

        reviveEvent.clickAction += Revive;
        cancelEvent.clickAction += CancelRevive;

        if (GameManager.Instance.revived)
        {
            revivePanel.SetActive(false);
            ShowResultPanel();
        }
        else
        {
            endPanel.SetActive(false);
            newScorePanel.SetActive(false);
            beforeBestScoreText.text = $"{GameManager.Instance.bestScore}";
            revivePanel.transform.localScale = Vector3.zero;
            fillCor = StartCoroutine(FillCounter());
            revivePanel.SetActive(true);
            revivePanel.transform.DOScale(Vector3.one, 0.3f);
        }
    }

    IEnumerator FillCounter()
    {
        float time = fillTime;
        fill.fillAmount = 1;
        while (true)
        {
            yield return null;
            if(time > 0)
            {
                time -= Time.deltaTime;
                fill.fillAmount = time / fillTime;
            }
            else
            {
                revivePanel.SetActive(false);
                ShowResultPanel();
                yield break;
            }
        }
    }

    private bool CheckBestScore()
    {
        return GameManager.Instance.Score > GameManager.Instance.bestScore;
    }

    void ShowResultPanel()
    {
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
            beforeBestTitle.SetActive(true);
            CloseBestScorePanel();
            beforeBestScoreText.text = $"{GameManager.Instance.bestScore}";
        }
        ReportScore(GameManager.Instance.Score);
        IncrementAchievement(GameManager.Instance.Score);
    }

    public void IncrementAchievement(int score)
    {
        PlayGamesPlatform.Instance.IncrementAchievement("CgkI4a37hcUTEAIQAg", score, (bool success) =>
        {
            if (success) Debug.Log("업적 진행도 업데이트 완료");
        });
    }

    public void ReportScore(long score)
    {
        // GPGSIds.leaderboard_rank는 Setup 시 자동 생성된 ID 클래스입니다.
        // 직접 문자열을 넣으려면 "CgkI..." 형태의 ID를 넣으세요.
        PlayGamesPlatform.Instance.ReportScore(score, "CgkI4a37hcUTEAIQAQ", (bool success) =>
        {
            if (success) Debug.Log("리더보드 점수 등록 성공: " + score);
            else Debug.LogWarning("리더보드 점수 등록 실패");
        });
    }



    /// <summary>
    /// 최고 점수 패널 닫고 종료 패널 열기
    /// </summary>
    public void CloseBestScorePanel()
    {
        SoundManager.Instance.PlayUI(clickSound);
        newScorePanel.SetActive(false);
        endPanel.SetActive(true);
    }

    /// <summary>
    /// 게임 재시작
    /// </summary>
    public void ResetGame()
    {
        GameManager.Instance.revived = false;
        SceneManager.LoadScene("GameScene");
    }

    private void Revive()
    {
        GameManager.Instance.revived = true;
        GameManager.Instance.isGameOver = false;
        GameManager.Instance.showAds = true;
        TileManager.Instance.ResetMap();
        Destroy(gameObject);
    }

    private void CancelRevive()
    {
        SoundManager.Instance.PlayUI(clickSound);
        ShowResultPanel();
        StopCoroutine(fillCor);
        Destroy(revivePanel);
    }
}
