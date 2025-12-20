using TMPro;
using UnityEngine;

public class UI_Coin : MonoBehaviour
{
    TextMeshProUGUI coinText;

    private void Start()
    {
        coinText = Util.FindChild<TextMeshProUGUI>(gameObject, "Coin");
        GameManager.Instance.coinAction += SetCoinUI;
        SetCoinUI(GameManager.Instance.Coin);
    }

    private void OnDisable()
    {
        GameManager.Instance.coinAction -= SetCoinUI;
    }

    private void SetCoinUI(int coin)
    {
        coinText.text = $"{coin}";
    }
}
