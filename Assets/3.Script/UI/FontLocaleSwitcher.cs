using UnityEngine;
using TMPro;

public class FontLocaleSwitcher : MonoBehaviour
{
   
    TextMeshProUGUI text;

    void Awake()
    {
        LocalizationManager.Instance.OnLanguageChanged += OnLocaleChanged;
        text = GetComponent<TextMeshProUGUI>();
        OnLocaleChanged();
    }

    void OnLocaleChanged()
    {
        var fontAsset = LocalizationManager.Instance.GetFontAsset();

        text.font = fontAsset;
    }
}