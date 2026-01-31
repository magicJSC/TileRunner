using UnityEngine;
using UnityEngine.SceneManagement;
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

    private string changeLanguage;

    [SerializeField] AudioClip clickSound;
    private void Start()
    {
        koreanClick.clickAction = OnClickKorean;
        englishClick.clickAction = OnClickEnglish;
        japaneseClick.clickAction = OnClickJapanese;

        GameManager.Instance.changeSettingAction += ChangeSetting;

        SetSprite();
    }

    public void OnClickKorean()
    {
        changeLanguage = "ko";
        SoundManager.Instance.PlayUI(clickSound);
        GameManager.Instance.beforeChangeSettingAction?.Invoke();
    }

    public void OnClickEnglish()
    {
        changeLanguage = "en";
        SoundManager.Instance.PlayUI(clickSound);
        GameManager.Instance.beforeChangeSettingAction?.Invoke();
    }

    public void OnClickJapanese()
    {
        changeLanguage = "ja";
        SoundManager.Instance.PlayUI(clickSound);
        GameManager.Instance.beforeChangeSettingAction?.Invoke();
    }

    void ChangeSetting()
    {
        LocalizationManager.Instance.SetLanguage(changeLanguage);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
