using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer mixer;

    [Header("Buttons")]
    public UI_EventHandler masterBtn;
    public UI_EventHandler bgmBtn;
    public UI_EventHandler sfxBtn;
    public UI_EventHandler uiBtn;

    [Header("Button Images")]
    public Image masterImg;
    public Image bgmImg;
    public Image sfxImg;
    public Image uiImg;
    public Image masterIcon;
    public Image bgmIcon;
    public Image sfxIcon;
    public Image uiIcon;

    [Header("Sprites")]
    public Sprite onSprite;
    public Sprite offSprite;
    public Sprite onIcon;
    public Sprite offIcon;

    const float ON = 0f;
    const float OFF = -80f;

    bool masterOn = true;
    bool bgmOn = true;
    bool sfxOn = true;
    bool uiOn = true;

    void Start()
    {
        masterBtn.clickAction = OnMasterClick;
        bgmBtn.clickAction = OnBGMClick;
        sfxBtn.clickAction = OnSFXClick;
        uiBtn.clickAction = OnUIClick;

        ApplyAll();
    }

    #region Button Clicks

    void OnMasterClick()
    {
        masterOn = !masterOn;

        bgmOn = masterOn;
        sfxOn = masterOn;
        uiOn = masterOn;

        ApplyAll();
    }

    void OnBGMClick()
    {
        ToggleSingle(ref bgmOn, "BGMVol");
    }

    void OnSFXClick()
    {
        ToggleSingle(ref sfxOn, "SFXVol");
    }

    void OnUIClick()
    {
        ToggleSingle(ref uiOn, "UIVol");
    }

    #endregion

    #region Core Logic

    void ToggleSingle(ref bool target, string param)
    {
        target = !target;

        if (target)
        {
            // 전체 OFF 상태에서 하나 켜면
            masterOn = true;
        }
        else
        {
            // 전부 OFF면 Master도 OFF
            if (!bgmOn && !sfxOn && !uiOn)
                masterOn = false;
        }

        ApplyAll();
    }

    void ApplyAll()
    {
        SetVolume("MasterVol", masterOn);
        SetVolume("BGMVol", bgmOn);
        SetVolume("SFXVol", sfxOn);
        SetVolume("UIVol", uiOn);

        UpdateIcon(masterImg,masterIcon, masterOn);
        UpdateIcon(bgmImg, bgmIcon, bgmOn);
        UpdateIcon(sfxImg, sfxIcon,sfxOn);
        UpdateIcon(uiImg, uiIcon,uiOn);
    }

    void SetVolume(string param, bool isOn)
    {
        mixer.SetFloat(param, isOn ? ON : OFF);
    }

    void UpdateIcon(Image img,Image icon, bool isOn)
    {
        img.sprite = isOn ? onSprite : offSprite;
        icon.sprite = isOn ? onIcon : offIcon;
    }

    #endregion
}
