using TMPro;
using UnityEngine;

public class StorePage : MonoBehaviour
{
    TextMeshProUGUI coinText;

    private void Start()
    {
        coinText = Util.FindChild<TextMeshProUGUI>(gameObject,"Coin");

        coinText.text = $"{GameManager.Instance.Coin}";
    }
}
