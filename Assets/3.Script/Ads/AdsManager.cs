using GoogleMobileAds.Api;
using UnityEngine;

public class AdsManager : Singleton<AdsManager>
{
    // �׽�Ʈ�� ID�Դϴ�. ���� ���� �ÿ��� AdMob �ܼ��� ID�� ��ü�ϼ���!
#if UNITY_ANDROID
   #if UNITY_EDITOR
private string _adUnitId = "ca-app-pub-3940256099942544/5224354917"; // 테스트 광고
#else
private string _adUnitId = "ca-app-pub-6017085359919621/3370759866";
#endif
#elif UNITY_IPHONE
        private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        private string _adUnitId = "unused";
#endif

    private InterstitialAd _interstitialAd;

    void Start()
    {
        // SDK �ʱ�ȭ
        MobileAds.Initialize((InitializationStatus status) => {
            LoadInterstitialAd();
        });
    }

    public void LoadInterstitialAd()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        var adRequest = new AdRequest();
        InterstitialAd.Load(_adUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("���� ���� �ε� ����: " + error);
                return;
            }
            _interstitialAd = ad;

            // ������ ������ �� ���� ������ �̸� �ε��ϴ� �̺�Ʈ ���
            _interstitialAd.OnAdFullScreenContentClosed += () => {
                Debug.Log("������ �������ϴ�. �� ������ �ε��մϴ�.");
                LoadInterstitialAd();
            };
        });
    }

    public bool CanShowAd()
    {
        return _interstitialAd != null && _interstitialAd.CanShowAd();
    }

    public void ShowAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("������ ǥ���մϴ�.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.Log("������ ���� �ε���� �ʾҽ��ϴ�.");
            LoadInterstitialAd(); // ���� ��� �ٽ� �ε�
        }
    }
}
