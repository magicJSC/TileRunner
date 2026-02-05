using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizationManager : Singleton<LocalizationManager>
{
    public Action OnLanguageChanged;

    public TMP_FontAsset fontEnglish;
    public TMP_FontAsset fontKorean;
    public TMP_FontAsset fontJapanese;
    public TMP_FontAsset fontChinese;

    private void Start()
    {
        StartCoroutine(InitLocale());
    }

    IEnumerator InitLocale()
    {
        // Localization 시스템 초기화 대기
        yield return LocalizationSettings.InitializationOperation;

        // 저장된 언어 불러오기
        if (PlayerPrefs.HasKey("LANG"))
        {
            string localeCode = PlayerPrefs.GetString("LANG");
            SetLanguage(localeCode);
        }
    }

    public void SetLanguage(string localeCode)
    {
        var locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);
        if (locale == null)
        {
            Debug.LogWarning($"Locale not found: {localeCode}");
            return;
        }

        LocalizationSettings.SelectedLocale = locale;
        PlayerPrefs.SetString("LANG", localeCode);
        OnLanguageChanged?.Invoke();
        PlayerPrefs.Save();
    }

    public string GetCurrentLanguageCode()
    {
        return LocalizationSettings.SelectedLocale.Identifier.Code;
    }

    public TMP_FontAsset GetFontAsset()
    {
        TMP_FontAsset selected = GetCurrentLanguageCode() switch
        {
            "ko" => fontKorean,
            "ja" => fontJapanese,
            "en" => fontEnglish,
            _ => fontEnglish
        };

        return selected;
    }
}
