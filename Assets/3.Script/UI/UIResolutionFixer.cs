using UnityEngine;
using UnityEngine.UI;

public class UIResolutionFixer : MonoBehaviour
{
    void Start()
    {
        CanvasScaler scaler = GetComponent<CanvasScaler>();

        // 기준 해상도 비율 (1080 / 2560 = 0.421)
        float targetAspect = scaler.referenceResolution.x / scaler.referenceResolution.y;

        // 현재 기기 비율
        float currentAspect = (float)Screen.width / Screen.height;

        if (currentAspect > targetAspect)
        {
            // 패드처럼 가로가 더 넓은 경우 -> 세로(Height)에 맞춤 (값: 1)
            scaler.matchWidthOrHeight = 1f;
        }
        else
        {
            // 폰처럼 세로가 더 긴 경우 -> 가로(Width)에 맞춤 (값: 0)
            scaler.matchWidthOrHeight = 0f;
        }
    }
}