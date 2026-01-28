using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFader : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] float fadeDuration = 0.3f;

    List<Image> images = new List<Image>();
    List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();

    void Awake()
    {
        // 자식 포함 전부 가져오기
        images.AddRange(GetComponentsInChildren<Image>(true));
        texts.AddRange(GetComponentsInChildren<TextMeshProUGUI>(true));
    }

    public void FadeIn()
    {
        Fade(1f);
    }

    public void FadeOut()
    {
        Fade(0f);
    }

    public void TurnOff()
    {
        foreach (var img in images)
        {
            if (img == null) continue;

            img.color = new Color(img.color.r, img.color.r, img.color.r, 0);
        }

        foreach (var text in texts)
        {
            if (text == null) continue;

            text.color = new Color(text.color.r, text.color.r, text.color.r, 0);
        }
    }

    void Fade(float targetAlpha)
    {
        foreach (var img in images)
        {
            if (img == null) continue;

            img.raycastTarget = false;

            img.DOKill();
            img.DOFade(targetAlpha, fadeDuration)
               .SetLink(img.gameObject).OnComplete(() => { img.raycastTarget = true; });
        }

        foreach (var text in texts)
        {
            if (text == null) continue;

            text.DOKill();
            text.DOFade(targetAlpha, fadeDuration)
                .SetLink(text.gameObject);
        }
    }
}
