using UnityEngine;
using TMPro;

public class FontLocaleSwitcher : MonoBehaviour
{
   
    TextMeshProUGUI text;

    void Start()
    {
        LocalizationManager.Instance.OnLanguageChanged += OnLocaleChanged;
        text = GetComponent<TextMeshProUGUI>();
        OnLocaleChanged();
    }

    private void OnDisable()
    {
        LocalizationManager.Instance.OnLanguageChanged -= OnLocaleChanged;
    }

    void OnLocaleChanged()
    {
        var fontAsset = LocalizationManager.Instance.GetFontAsset();

        text.font = fontAsset;
    }
}