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
            revivePanel.SetActive(true);
            revivePanel.transform.DOScale(Vector3.one, 0.3f);
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

        SetAchievementProgress(GameManager.Instance.bestScore);
        ReportScore(GameManager.Instance.bestScore);
    }

    public void SetAchievementProgress(int currentScore)
    {
        int targetScore = 400; // 예시 목표 점수

        // 1. 점수를 퍼센트(0.0 ~ 100.0)로 변환
        // 주의: 정수 나눗셈 방지를 위해 하나는 double로 형변환 필수
        double progress = ((double)currentScore / targetScore) * 100.0;

        // 2. PlayGamesPlatform 인스턴스를 통한 리포트
        PlayGamesPlatform.Instance.ReportProgress("CgkI4a37hcUTEAIQAg", progress, (bool success) =>
        {
            if (success)
            {
                Debug.Log($"[GPGS] \"CgkI4a37hcUTEAIQAg\" 업적 진행도 {progress:F1}% 설정 완료");
            }
            else
            {
                Debug.LogError("[GPGS] 업적 업데이트 실패");
            }
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
        revivePanel.SetActive(false);
        StopCoroutine(fillCor);
        Destroy(revivePanel);
    }
}
