using UnityEngine;
using UnityEngine.UI;

public class UI_Boss : MonoBehaviour
{
    private Image fillImage; 
    void Start()
    {
        fillImage = Util.FindChild<Image>(gameObject, "Fill");

        BossController.Instance.bossGageAction += UpdateBossGage;
        UpdateBossGage(0f);
    }

    private void UpdateBossGage(float radio)
    {
        fillImage.fillAmount = radio;
    }
}
