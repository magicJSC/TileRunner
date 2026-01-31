using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    [Header("Buttons")]
    public UI_EventHandler masterBtn;
    public UI_EventHandler bgmBtn;
    public UI_EventHandler sfxBtn;
    public UI_EventHandler uiBtn;
    [SerializeField] AudioClip clickSound;

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
        SoundManager.Instance.masterOn = !SoundManager.Instance.masterOn;

        SoundManager.Instance.bgmOn = SoundManager.Instance.masterOn;
        SoundManager.Instance.sfxOn = SoundManager.Instance.masterOn;
        SoundManager.Instance.uiOn = SoundManager.Instance.masterOn;

        ApplyAll();
        SoundManager.Instance.PlayUI(clickSound);
    }

    void OnBGMClick()
    {
        ToggleSingle(ref SoundManager.Instance.bgmOn, "BGMVol");
        SoundManager.Instance.PlayUI(clickSound);
    }

    void OnSFXClick()
    {
        ToggleSingle(ref SoundManager.Instance.sfxOn, "SFXVol");
        SoundManager.Instance.PlayUI(clickSound);
    }

    void OnUIClick()
    {
        ToggleSingle(ref SoundManager.Instance.uiOn, "UIVol");
        SoundManager.Instance.PlayUI(clickSound);
    }

    #endregion

    #region Core Logic

    

    void ApplyAll()
    {
        SoundManager.Instance.SetAllVolume();

        UpdateIcon(masterImg,masterIcon, SoundManager.Instance.masterOn);
        UpdateIcon(bgmImg, bgmIcon, SoundManager.Instance.bgmOn);
        UpdateIcon(sfxImg, sfxIcon,SoundManager.Instance.sfxOn);
        UpdateIcon(uiImg, uiIcon,SoundManager.Instance.uiOn);
    }

   

    void UpdateIcon(Image img,Image icon, bool isOn)
    {
        img.sprite = isOn ? onSprite : offSprite;
        icon.sprite = isOn ? onIcon : offIcon;
    }

    public void ToggleSingle(ref bool target, string param)
    {
        target = !target;

        if (target)
        {
            // 전체 OFF 상태에서 하나 켜면
            SoundManager.Instance.masterOn = true;
        }
        else
        {
            // 전부 OFF면 Master도 OFF
            if (!SoundManager.Instance.bgmOn && !SoundManager.Instance.sfxOn && !SoundManager.Instance.uiOn)
                SoundManager.Instance.masterOn = false;
        }

        ApplyAll();
    }
    #endregion
}
