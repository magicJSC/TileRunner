using DG.Tweening;
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

    private RectTransform shopBtnRect;
    private RectTransform gameBtnRect;
    private RectTransform rankBtnRect;

    [SerializeField] AudioClip clickSound;

    [Header("Page")]
    [SerializeField] GameObject shopPage;
    [SerializeField] GameObject gamePage;
    [SerializeField] GameObject rankPage;

    Transform pageParent;
    int curIndex = 0;
    GameObject curPage;
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

        shopBtnRect = shopBtn.GetComponent<RectTransform>();
        gameBtnRect = gameBtn.GetComponent<RectTransform>();
        rankBtnRect = rankBtn.GetComponent<RectTransform>();

        pageParent = Util.FindChild<Transform>(gameObject, "PageParent");
        curPage = pageParent.GetChild(0).gameObject;

        shopBtn.sprite = unClickSprite;
        gameBtn.sprite = clickSprite;
        rankBtn.sprite = unClickSprite;

        //shopBtnRect.anchoredPosition = new Vector2(shopBtnRect.anchoredPosition.x, -870);
        //gameBtnRect.anchoredPosition = new Vector2(gameBtnRect.anchoredPosition.x, -850);
        //rankBtnRect.anchoredPosition = new Vector2(rankBtnRect.anchoredPosition.x, -870);

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
        shopBtn.sprite = unClickSprite;
        gameBtn.sprite = unClickSprite;
        rankBtn.sprite = clickSprite;

        SoundManager.Instance.PlayUI(clickSound);

        ChangePage(1, rankPage);
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
