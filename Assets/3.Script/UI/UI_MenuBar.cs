using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_MenuBar : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] Sprite clickSprite;
    [SerializeField] Sprite unClickSprite;

    private Image shopBtn;
    private Image gameBtn;
    private Image missionBtn;

    [Header("Page")]

    [SerializeField] GameObject shopPage;
    [SerializeField] GameObject gamePage;
    [SerializeField] GameObject missionPage;

    Transform pageParent;
    int curIndex = 0;
    GameObject curPage;
    bool isChanging = false;


    private void Start()
    {
        pageParent = Util.FindChild<Transform>(gameObject, "PageParent");
        curPage = pageParent.GetChild(0).gameObject;

        AddBtnAction();
    }

    void AddBtnAction()
    {
        shopBtn = Util.FindChild<Image>(gameObject, "ShopBtn");
        gameBtn = Util.FindChild<Image>(gameObject, "GameBtn");
        missionBtn = Util.FindChild<Image>(gameObject, "MissionBtn");

        UI_EventHandler evt = shopBtn.GetComponent<UI_EventHandler>();
        evt.clickAction = ShopPage;

        evt = gameBtn.GetComponent<UI_EventHandler>();
        evt.clickAction = GamePage;

        evt = missionBtn.GetComponent<UI_EventHandler>();
        evt.clickAction = MissionPage;
    }

    /// <summary>
    /// 상점 페이지 클릭 함수
    /// </summary>
    void ShopPage()
    {
        shopBtn.sprite = clickSprite;
        gameBtn.sprite = unClickSprite;
        missionBtn.sprite = unClickSprite;

        ChangePage(-1, shopPage);
    }

    /// <summary>
    /// 게임 페이지 클릭 함수
    /// </summary>
    void GamePage()
    {
        shopBtn.sprite = unClickSprite;
        gameBtn.sprite = clickSprite;
        missionBtn.sprite = unClickSprite;

        ChangePage(0, gamePage);
    }

    /// <summary>
    /// 미션 페이지 클릭 함수
    /// </summary>
    void MissionPage()
    {
        shopBtn.sprite = unClickSprite;
        gameBtn.sprite = unClickSprite;
        missionBtn.sprite = clickSprite;

        ChangePage(1, missionPage);
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
