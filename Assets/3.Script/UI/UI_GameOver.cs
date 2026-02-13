using DG.Tweening;
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
            bestScoreText.text = $"{GameManager.Instance.Score}";
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
