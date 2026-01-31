using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
   public static SoundManager Instance;

    [Header("Audio Mixer")]
    [SerializeField] AudioMixer mixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource uiSource;

    const float ON = 0f;
    const float OFF = -80f;

    public bool masterOn = true;
    public bool bgmOn = true;
    public bool sfxOn = true;
    public bool uiOn = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadVolume();
    }

    private void OnDisable()
    {
        SetVolume();
    }
   

    public void LoadVolume()
    {
        if (PlayerPrefs.HasKey("MasterVol"))
        {
            masterOn = PlayerPrefs.GetInt("MasterVol") == 1;
            bgmOn = PlayerPrefs.GetInt("BgmVol") == 1;
            sfxOn = PlayerPrefs.GetInt("SfxVol") == 1;
            uiOn = PlayerPrefs.GetInt("UIVol") == 1;
        }
        else
        {
            PlayerPrefs.SetInt("MasterVol", masterOn ? 1 : 0);
            PlayerPrefs.SetInt("BgmVol", bgmOn ? 1 : 0);
            PlayerPrefs.SetInt("SfxVol", sfxOn ? 1 : 0);
            PlayerPrefs.SetInt("UIVol", uiOn ? 1 : 0);
        }
        SetAllVolume();
    }

    public void SetVolume()
    {
        PlayerPrefs.SetInt("MasterVol", masterOn ? 1 : 0);
        PlayerPrefs.SetInt("BgmVol", bgmOn ? 1 : 0);
        PlayerPrefs.SetInt("SfxVol", sfxOn ? 1 : 0);
        PlayerPrefs.SetInt("UIVol", uiOn ? 1 : 0);
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayUI(AudioClip clip)
    {
        uiSource.PlayOneShot(clip);
    }


    #region Setting

    public void SetVolume(string param, bool isOn)
    {
        mixer.SetFloat(param, isOn ? ON : OFF);
    }

   

    public void SetAllVolume()
    {
        SetVolume("MasterVol", masterOn);
        SetVolume("BGMVol", bgmOn);
        SetVolume("SFXVol", sfxOn);
        SetVolume("UIVol", uiOn);
    }

    #endregion
}
