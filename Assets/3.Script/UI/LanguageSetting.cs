using UnityEngine;
using UnityEngine.UI;

public class LanguageSetting : MonoBehaviour
{
    [Header("Spirte")]
    [SerializeField] Image koreanImage;
    [SerializeField] Image englishImage;
    [SerializeField] Image japaneseImage;

    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;

    [Header("Click Event")]
    [SerializeField] UI_EventHandler koreanClick;
    [SerializeField] UI_EventHandler englishClick;
    [SerializeField] UI_EventHandler japaneseClick;

    [SerializeField] AudioClip clickSound;
    private void Start()
    {
        koreanClick.clickAction = OnClickKorean;
        englishClick.clickAction = OnClickEnglish;
        japaneseClick.clickAction = OnClickJapanese;

        SetSprite();
    }

    public void OnClickKorean()
    {
        LocalizationManager.Instance.SetLanguage("ko");
        SoundManager.Instance.PlayUI(clickSound);
        SetSprite();
    }

    public void OnClickEnglish()
    {
        LocalizationManager.Instance.SetLanguage("en");
        SoundManager.Instance.PlayUI(clickSound);
        SetSprite();
    }

    public void OnClickJapanese()
    {
        LocalizationManager.Instance.SetLanguage("ja");
        SoundManager.Instance.PlayUI(clickSound);
        SetSprite();
    }

    void SetSprite()
    {
        string code = LocalizationManager.Instance.GetCurrentLanguageCode();
        switch (code)
        {
            case "ko":
                koreanImage.sprite = onSprite;
                englishImage.sprite = offSprite;
                japaneseImage.sprite = offSprite;
                break;
            case "ja":
                koreanImage.sprite = offSprite;
                englishImage.sprite = offSprite;
                japaneseImage.sprite = onSprite;
                break;
            case "en":
                koreanImage.sprite = offSprite;
                englishImage.sprite = onSprite;
                japaneseImage.sprite = offSprite;
                break;
        }
    }
}
