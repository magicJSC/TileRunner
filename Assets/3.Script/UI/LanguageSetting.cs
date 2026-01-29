using UnityEngine;

public class LanguageSetting : MonoBehaviour
{
    [SerializeField] UI_EventHandler koreanClick;
    [SerializeField] UI_EventHandler englishClick;
    [SerializeField] UI_EventHandler japaneseClick;

    private void Start()
    {
        koreanClick.clickAction = OnClickKorean;
        englishClick.clickAction = OnClickEnglish;
        japaneseClick.clickAction = OnClickJapanese;
    }

    public void OnClickKorean()
    {
        LocalizationManager.Instance.SetLanguage("ko");
    }

    public void OnClickEnglish()
    {
        LocalizationManager.Instance.SetLanguage("en");
    }

    public void OnClickJapanese()
    {
        LocalizationManager.Instance.SetLanguage("ja");
    }
}
