using GoogleMobileAds.Api;
using UnityEngine;

public class AdsManager : Singleton<AdsManager>
{
    // 테스트용 ID입니다. 실제 배포 시에는 AdMob 콘솔의 ID로 교체하세요!
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
        private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        private string _adUnitId = "unused";
#endif

    private InterstitialAd _interstitialAd;

    void Start()
    {
        // SDK 초기화
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
                Debug.LogError("전면 광고 로드 실패: " + error);
                return;
            }
            _interstitialAd = ad;

            // 광고가 닫혔을 때 다음 광고를 미리 로드하는 이벤트 등록
            _interstitialAd.OnAdFullScreenContentClosed += () => {
                Debug.Log("광고가 닫혔습니다. 새 광고를 로드합니다.");
                LoadInterstitialAd();
            };
        });
    }

    public void ShowAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("광고를 표시합니다.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.Log("광고가 아직 로드되지 않았습니다.");
            LoadInterstitialAd(); // 실패 대비 다시 로드
        }
    }
}
