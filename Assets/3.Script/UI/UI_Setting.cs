using GooglePlayGames;
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

    }

    private void OnEnable()
    {
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
        ShowAchievementUI();
        SoundManager.Instance.PlayUI(clickClip);
    }

    public void ShowAchievementUI()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            SetAchievementProgress(GameManager.Instance.bestScore);
            PlayGamesPlatform.Instance.ShowAchievementsUI();
        }
        else
        {
            Debug.Log("로그인이 필요합니다.");
        }
    }

    public void SetAchievementProgress(int currentScore)
    {
        int targetScore = 400; // 예시 목표 점수

        // 1. 점수를 퍼센트(0.0 ~ 100.0)로 변환
        // 주의: 정수 나눗셈 방지를 위해 하나는 double로 형변환 필수
        double progress = ((double)currentScore / targetScore) * 100.0;

        // 2. PlayGamesPlatform 인스턴스를 통한 리포트
        PlayGamesPlatform.Instance.ReportProgress("CgkI4a37hcUTEAIQAg", progress, (bool success) =>
        {
            if (success)
            {
                Debug.Log($"[GPGS] \"CgkI4a37hcUTEAIQAg\" 업적 진행도 {progress:F1}% 설정 완료");
            }
            else
            {
                Debug.LogError("[GPGS] 업적 업데이트 실패");
            }
        });
    }

    void Disappear()
    {
        gameObject.SetActive(false);
    }
}
