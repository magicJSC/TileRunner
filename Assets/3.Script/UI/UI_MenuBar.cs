using DG.Tweening;
using GooglePlayGames;
using UnityEngine;
using UnityEngine.UI;

public class UI_MenuBar : MonoBehaviour
{
    private Animator anim;

    [Header("Button")]
    [SerializeField] Sprite clickSprite;
    [SerializeField] Sprite unClickSprite;

    private Image shopBtn;
    private Image gameBtn;
    private Image rankBtn;

    [SerializeField] AudioClip clickSound;

    [Header("Page")]
    [SerializeField] GameObject shopPage;
    [SerializeField] GameObject gamePage;
    [SerializeField] GameObject rankPage;

    [SerializeField] Transform pageParent;
    int curIndex = 0;
    [SerializeField] GameObject curPage;
    bool isChanging = false;


    private void Start()
    {
        InitSettings();
        AddBtnAction();
    }

    void InitSettings()
    {
        // 초기 설정이 필요하면 여기에 작성
        anim = GetComponent<Animator>();

        shopBtn = Util.FindChild<Image>(gameObject, "ShopBtn");
        gameBtn = Util.FindChild<Image>(gameObject, "GameBtn");
        rankBtn = Util.FindChild<Image>(gameObject, "RankBtn");

        shopBtn.sprite = unClickSprite;
        gameBtn.sprite = clickSprite;
        rankBtn.sprite = unClickSprite;

        GameManager.Instance.startGameAction += StartGame;
    }

    private void OnDisable()
    {
        GameManager.Instance.startGameAction -= StartGame;
    }

    void AddBtnAction()
    {
        UI_EventHandler evt = shopBtn.GetComponent<UI_EventHandler>();
        evt.clickAction = ShopPage;

        evt = gameBtn.GetComponent<UI_EventHandler>();
        evt.clickAction = GamePage;

        evt = rankBtn.GetComponent<UI_EventHandler>();
        evt.clickAction = RankPage;
    }

    /// <summary>
    /// 상점 페이지 클릭 함수
    /// </summary>
    void ShopPage()
    {
        shopBtn.sprite = clickSprite;
        gameBtn.sprite = unClickSprite;
        rankBtn.sprite = unClickSprite;

        SoundManager.Instance.PlayUI(clickSound);

        ChangePage(-1, shopPage);
    }

    /// <summary>
    /// 게임 페이지 클릭 함수
    /// </summary>
    void GamePage()
    {
        shopBtn.sprite = unClickSprite;
        gameBtn.sprite = clickSprite;
        rankBtn.sprite = unClickSprite;

        SoundManager.Instance.PlayUI(clickSound);

        ChangePage(0, gamePage);
    }

    /// <summary>
    /// 미션 페이지 클릭 함수
    /// </summary>
    void RankPage()
    {
        ShowLeaderboardUI();

        SoundManager.Instance.PlayUI(clickSound);

    }

    public void ShowLeaderboardUI()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            ReportScore(GameManager.Instance.bestScore);
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        }
        else
        {
            Debug.Log("로그인이 필요합니다.");
        }
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
    /// 페이지 변경 함수
    /// </summary>
    /// <param name="changeIndex"></param>
    /// <param name="pagePrefab"></param>
    void ChangePage(int changeIndex, GameObject pagePrefab)
    {
        if(curIndex == changeIndex || isChanging)
            return;

        isChanging = true;

        GameObject changePageGo = Instantiate(pagePrefab, pageParent);

        RectTransform changePageRect = changePageGo.GetComponent<RectTransform>();
        RectTransform curPageRect = curPage.GetComponent<RectTransform>();

        if (curIndex > changeIndex)
        {
            changePageRect.anchoredPosition = new Vector2(-1080, 0);
            changePageRect.DOAnchorPos(new Vector2(0,0),0.1f);
            curPageRect.DOAnchorPos(new Vector2(1080,0),0.1f).OnComplete(() =>
            {
                EndChange(changePageGo);
            });
        }
        else
        {
            changePageRect.anchoredPosition = new Vector2(1080, 0);
            changePageRect.DOAnchorPos(new Vector2(0, 0), 0.1f);
            curPageRect.DOAnchorPos(new Vector2(-1080, 0), 0.1f).OnComplete(() =>
            {
                EndChange(changePageGo);
            });
        }
        curIndex = changeIndex;
    }

    void StartGame()
    {
        anim.Play("Start");
    }

    /// <summary>
    /// 페이지 변경 완료 함수
    /// </summary>
    void EndChange(GameObject changePage)
    {
        isChanging = false;
        Destroy(curPage);
        curPage = changePage;
    }
}
