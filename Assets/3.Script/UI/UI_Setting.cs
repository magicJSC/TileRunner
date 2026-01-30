using System;
using UnityEngine;

public class UI_Setting : MonoBehaviour
{
    Animator anim;

    [SerializeField] GameObject settingPanel;
    [SerializeField] GameObject languagePanel;
    [SerializeField] GameObject audioPanel;

    [SerializeField] UI_EventHandler backBtn;

    [SerializeField] UI_EventHandler languageBtn;
    [SerializeField] UI_EventHandler audioBtn;
    [SerializeField] UI_EventHandler acheiveBtn;

    [SerializeField] UIFader settingFade;
    [SerializeField] UIFader languageFade;
    [SerializeField] UIFader audioFade;

    [SerializeField] AudioClip clickClip;

    private void Start()
    {
        anim = GetComponent<Animator>();

        backBtn.clickAction = ClickSettingBack;

        languageBtn.clickAction = ClickLanguageBtn;
        audioBtn.clickAction = ClickAudioBtn;
        acheiveBtn.clickAction = ClickAchieveBtn;

        settingFade.TurnOff();
        languageFade.TurnOff();
        audioFade.TurnOff();

        settingFade.FadeIn();
    }


    void ClickSettingBack()
    {
        GameManager.Instance.closeSettingAction?.Invoke();
        settingFade.FadeOut();

        SoundManager.Instance.PlayUI(clickClip);
        anim.Play("Back");

    }

    void ClickLauguageBack()
    {
        settingPanel.SetActive(true);
        languagePanel.SetActive(false);

        settingFade.FadeIn();
        languageFade.FadeOut();
        backBtn.clickAction = ClickSettingBack;

        SoundManager.Instance.PlayUI(clickClip);
    }

    void ClickAudioBack()
    {
        settingPanel.SetActive(true);
        audioPanel.SetActive(false);

        settingFade.FadeIn();
        audioFade.FadeOut();
        backBtn.clickAction = ClickSettingBack;

        SoundManager.Instance.PlayUI(clickClip);
    }

    void ClickLanguageBtn()
    {
        languagePanel.SetActive(true);
        settingPanel.SetActive(false);

        languageFade.FadeIn();
        settingFade.FadeOut();
        backBtn.clickAction = ClickLauguageBack;

        SoundManager.Instance.PlayUI(clickClip);
    }

    void ClickAudioBtn()
    {
        audioPanel.SetActive(true);
        settingPanel.SetActive(false);

        audioFade.FadeIn();
        settingFade.FadeOut();
        backBtn.clickAction = ClickAudioBack;

        SoundManager.Instance.PlayUI(clickClip);
    }
    void ClickAchieveBtn()
    {
        SoundManager.Instance.PlayUI(clickClip);
    }

    public void Disappear()
    {
        Destroy(gameObject);
    }
}
