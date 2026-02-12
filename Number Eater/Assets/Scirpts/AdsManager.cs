using GoogleMobileAds.Api;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    [Header("Ad Unit")]
#if UNITY_ANDROID
    private const string AD_UNIT_ID = "ca-app-pub-9548284037151614/7163146748";
#elif UNITY_IOS
    private const string AD_UNIT_ID = "ca-app-pub-3940256099942544/4411468910";
#else
    private const string AD_UNIT_ID = "unused";
#endif

    [Header("First Time User Settings")]
    [SerializeField] private int firstUserNoAdClearCount = 3; // 처음 유저 보호 클리어 수

    [Header("Returning User Settings")]
    [SerializeField] private int forceAdClearCountForReturningUser = 2; // 복귀 유저 초반 강제 광고
    [SerializeField] private int adIntervalAfterForce = 3; // 이후 n판마다 광고

    private InterstitialAd interstitialAd;

    private int firstUserClearCount = 0;
    private int clearCountForReturningUser = 0;

    private const string HAS_PLAYED_BEFORE_KEY = "HAS_PLAYED_BEFORE";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // No Ads 구매했으면 광고 시스템 자체 비활성
        if (NoAdsManager.Instance.HasNoAds)
        {
            Debug.Log("[Ads] No Ads purchased - Ads disabled");
            return;
        }

        MobileAds.Initialize(_ => { });
        LoadInterstitial();
    }

    // =========================
    // 광고 로딩
    // =========================
    private void LoadInterstitial()
    {
        interstitialAd?.Destroy();
        interstitialAd = null;

        var request = new AdRequest();

        InterstitialAd.Load(AD_UNIT_ID, request,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.Log("[Ads] Interstitial load failed");
                    return;
                }

                interstitialAd = ad;

                interstitialAd.OnAdFullScreenContentClosed += () =>
                {
                    LoadInterstitial(); // 닫히면 다음 광고 미리 로드
                };
            });
    }

    // =========================
    // 게임 오버 → 무조건 광고
    // =========================
    public void OnGameOver()
    {
        if (NoAdsManager.Instance.HasNoAds)
            return;

        ShowInterstitial();
    }

    // =========================
    // 게임 클리어 → 조건별 분기
    // =========================
    public void OnGameClear()
    {
        if (NoAdsManager.Instance.HasNoAds)
            return;

        // 🟢 처음 플레이 유저
        if (IsFirstTimePlayer())
        {
            firstUserClearCount++;

            if (firstUserClearCount < firstUserNoAdClearCount)
            {
                Debug.Log($"[Ads] First user clear {firstUserClearCount} - no ad");
                return;
            }

            // 보호 구간 종료
            MarkUserAsPlayed();
            ShowInterstitial();
            return;
        }

        // 🔵 이미 플레이한 유저
        clearCountForReturningUser++;

        // 1️⃣ 초반 1~2번은 무조건 광고
        if (clearCountForReturningUser <= forceAdClearCountForReturningUser)
        {
            Debug.Log($"[Ads] Returning user force ad ({clearCountForReturningUser})");
            ShowInterstitial();
            return;
        }

        // 2️⃣ 이후부터는 n판마다 광고
        int afterForceCount = clearCountForReturningUser - forceAdClearCountForReturningUser;

        if (afterForceCount % adIntervalAfterForce == 0)
        {
            Debug.Log("[Ads] Returning user interval ad");
            ShowInterstitial();
        }
        else
        {
            Debug.Log("[Ads] Returning user skip ad");
        }
    }

    // =========================
    // 실제 광고 표시
    // =========================
    private void ShowInterstitial()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("[Ads] Interstitial not ready, reloading");
            LoadInterstitial();
        }
    }

    // =========================
    // 처음 플레이 여부 처리
    // =========================
    private bool IsFirstTimePlayer()
    {
        return PlayerPrefs.GetInt(HAS_PLAYED_BEFORE_KEY, 0) == 0;
    }

    private void MarkUserAsPlayed()
    {
        PlayerPrefs.SetInt(HAS_PLAYED_BEFORE_KEY, 1);
        PlayerPrefs.Save();
    }
}