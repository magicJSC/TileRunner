using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorePage : MonoBehaviour
{
    TextMeshProUGUI coinText;
    TextMeshProUGUI priceText;

    private Image characterBtn;
    private Image purchaseBtn;

    private int selectIndex = 0;

    private GameObject usingBack;
    private GameObject leftArrow;
    private GameObject rightArrow;

    private void Start()
    {
        coinText = Util.FindChild<TextMeshProUGUI>(gameObject,"Coin");
        priceText = Util.FindChild<TextMeshProUGUI>(gameObject,"Price");

        leftArrow = Util.FindChild(gameObject, "LeftArrow");
        rightArrow = Util.FindChild(gameObject, "RightArrow");

        GameManager.Instance.coinAction += SetCoin;
        SetCoin(GameManager.Instance.Coin);

        characterBtn = Util.FindChild<Image>(gameObject, "CharacterBtn");
        purchaseBtn = Util.FindChild<Image>(gameObject, "PurchaseBtn");
        usingBack = Util.FindChild(gameObject, "UsingBack");



        SwipeAction(CharacterManger.Instance.useIndex);
        InitAction();
    }

    private void OnDisable()
    {
        GameManager.Instance.coinAction -= SetCoin;
    }

    void InitAction()
    {
        UI_EventHandler evt = characterBtn.gameObject.GetComponent<UI_EventHandler>();
        evt.clickAction = UseCharacter;
        evt = purchaseBtn.gameObject.GetComponent<UI_EventHandler>();
        evt.clickAction = PurchaseCharacter;
        
        GetComponentInChildren<StoreSwipe>().swipeAction = SwipeAction;
    }

    void SwipeAction(int index)
    {
        selectIndex = index;

        if(index == 0)
            leftArrow.SetActive(false);
        else if(index == CharacterManger.Instance.characterPoints.Length - 1)
            rightArrow.SetActive(false);
        else
        {
            leftArrow.SetActive(true);
            rightArrow.SetActive(true);
        }

        ChangeUI();
    }

    /// <summary>
    /// 선택 인덱스가 바뀔 시 UI 변경
    /// </summary>
    void ChangeUI()
    {
        if(CharacterManger.Instance.IsCharacterUsable(selectIndex))
        {
            purchaseBtn.gameObject.SetActive(false);
            characterBtn.gameObject.SetActive(true);
            if(CharacterManger.Instance.useIndex == selectIndex)
            {
                usingBack.SetActive(true);
            }
            else
            {
                usingBack.SetActive(false);
            }
        }
        else
        {
            purchaseBtn.gameObject.SetActive(true);
            characterBtn.gameObject.SetActive(false);
            usingBack.SetActive(false);
            SetPrice();
        }
    }

    /// <summary>
    /// 선택 된 캐릭터로 usingCharacterIndex 변경
    /// </summary>
    void UseCharacter()
    {
        CharacterManger.Instance.useIndex = selectIndex;
        usingBack.SetActive(true);
        characterBtn.gameObject.SetActive(false);

        CharacterManger.Instance.ChangeCharacter(CharacterManger.Instance.useIndex);
    }

    void SetPrice()
    {
        priceText.text = $"{CharacterManger.Instance.GetCharacterData(selectIndex).money}";
    }

    void SetCoin(int coin)
    {
        coinText.text = $"{coin}";
    }

    /// <summary>
    /// 캐릭터 구입
    /// </summary>
    void PurchaseCharacter()
    {
        CharacterData data = CharacterManger.Instance.GetCharacterData(selectIndex);
        if(GameManager.Instance.Coin >= data.money)
        {
            GameManager.Instance.Coin -= data.money;

            CharacterManger.Instance.ChangeUsable(selectIndex);
            ChangeUI();
        }
    }
}
