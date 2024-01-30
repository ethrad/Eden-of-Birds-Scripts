using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class LoadMobileAD : MonoBehaviour
{
    public static LoadMobileAD Instance;
    private string _adUnitId;
    private string _RewardedAdUnitId;
    private string _BannerAdUnitId;
    private InterstitialAd _interstitialAd;
    private RewardedAd _rewardedAd;
    BannerView _bannerView;

    public int rewardType;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public float DebugBannerViewHeight()
    {
        return _bannerView.GetHeightInPixels();
    }

    void Start()
    {
        //Google 모바일AD SDK 초기화
        MobileAds.Initialize((InitializationStatus initStatus) => { });



        // 테스트 용
        #if UNITY_ANDROID
        _adUnitId = "ca-app-pub-3940256099942544/1033173712";
        _RewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
        _BannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
        #endif

        // 실사용
/*#if UNITY_ANDROID
        _adUnitId = "ca-app-pub-2330729664109938/7134249018";
        _RewardedAdUnitId = "ca-app-pub-2330729664109938/4887359628";
        _BannerAdUnitId = "ca-app-pub-2330729664109938/5446049207";
#endif*/
    }

    #region 전면 광고

    //전면 광고 로드
    public void LoadInterstitialAd()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
        Debug.Log("Loading the interstitial ad.");

        var adRequest = new AdRequest();
        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // error가 null이 아닌경우 Request 실패
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " + "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;
                RegisterEventHandlers(_interstitialAd);
                ShowInterstitialAd();
            });
    }

    public void LoadInterstitialAd(string sceneName)
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
        Debug.Log("Loading the interstitial ad.");

        var adRequest = new AdRequest();
        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // error가 null이 아닌경우 Request 실패
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " + "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;
                RegisterEventHandlers(_interstitialAd);

                StartCoroutine(ShowInterstitialAdDelay(sceneName));
            });
    }

    IEnumerator ShowInterstitialAdDelay(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);


        while (!op.isDone)
        {
            yield return null;
        }

        ShowInterstitialAd();
    }

    //전면 광고 게재
    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    //전면 광고 이벤트 핸들러
    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };

        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };

        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };

        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };

        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
        };

        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    #endregion

    #region 리워드 광고

    public void OnRewardedAd(int rewardType)
    {
        Time.timeScale = 0f;
        this.rewardType = rewardType;
        LoadRewardedAd();
        ShowRewardedAd();
    }

    //리워드 광고 로드
    void LoadRewardedAd()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        var adRequest = new AdRequest();

        RewardedAd.Load(_RewardedAdUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;

                //수정한 부분
                RegisterEventHandlers(_rewardedAd);
                ShowRewardedAd();
            });
    }

    //리워드 광고 게재
    void ShowRewardedAd()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            { // 유저가 받은 보상
                // TODO: Reward the user.

                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }

    //리워드 광고 이벤트 핸들러
    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };

        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };

        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };

        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };

        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            switch (rewardType)
            {
                case 0:
                    GameManager.instance.gameData.dungeonRemainCount++;
                    GameManager.instance.gameData.dungeonEnterAdCount++;
                    SceneManager.LoadScene("Field");

                    GameManager.instance.WriteGameData();
                    break;
                case 1:
                    Time.timeScale = 1f;
                    DungeonManager.instance.RevivePlayer();
                    break;
                case 2:
                    // 광고 보면 깃털 보상
                    break;
            }

            Time.timeScale = 1f;
        };

        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    //리워드 광고 닫을 때
    public GameObject rewardEndPanel;

    public void rewardedPopup()
    {
        Debug.Log("리워드 광고");
        rewardEndPanel.SetActive(true);
    }

    public void ExitRewardEndPanel()
    {
        rewardEndPanel.SetActive(false);
    }

    #endregion

    #region 배너 광고

    public void LoadBanner()
    {
        RequestConfiguration requestConfiguration = new RequestConfiguration
        {
            TestDeviceIds = new List<string>
            {
                AdRequest.TestDeviceSimulator,
                #if UNITY_ANDROID
                "75EF8D155528C04DACBBA6F36F433035"
                #endif
            }
        };
        MobileAds.SetRequestConfiguration(requestConfiguration);
        MobileAds.Initialize((InitializationStatus status) =>
        {
            RequestBanner();
        });
    }

    public void LoadBannerTop()
    {
        RequestConfiguration requestConfiguration = new RequestConfiguration
        {
            TestDeviceIds = new List<string>
            {
                AdRequest.TestDeviceSimulator,
                #if UNITY_ANDROID
                "75EF8D155528C04DACBBA6F36F433035"
                #endif
            }
        };
        MobileAds.SetRequestConfiguration(requestConfiguration);
        MobileAds.Initialize((InitializationStatus status) =>
        {
            RequestBannerTop();
        });
    }

    private void RequestBanner()
    {
        // 이미 배너가 있다면 이전것을 없앰
        if (_bannerView != null)
        {
            _bannerView.Destroy();
        }

        AdSize adaptiveSize =
            AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        _bannerView = new BannerView(_BannerAdUnitId, adaptiveSize, AdPosition.Bottom);

        //_bannerView.OnBannerAdLoaded += OnBannerAdLoaded;
        //_bannerView.OnBannerAdLoadFailed += OnBannerAdLoadFailed;

        AdRequest adRequest = new AdRequest();

        _bannerView.LoadAd(adRequest);
    }

    private void RequestBannerTop()
    {
        // 이미 배너가 있다면 이전것을 없앰
        if (_bannerView != null)
        {
            _bannerView.Destroy();
        }

        AdSize adaptiveSize =
            AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        _bannerView = new BannerView(_BannerAdUnitId, adaptiveSize, AdPosition.Top);

        //_bannerView.OnBannerAdLoaded += OnBannerAdLoaded;
        //_bannerView.OnBannerAdLoadFailed += OnBannerAdLoadFailed;

        AdRequest adRequest = new AdRequest();

        _bannerView.LoadAd(adRequest);
    }

    public void DestroyBannerView()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    #region Banner callback handlers

    private void OnBannerAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Banner view loaded an ad with response : "
                  + _bannerView.GetResponseInfo());
        Debug.Log("Ad Height: {0}, width: {1}" +
                  _bannerView.GetHeightInPixels() +
                  _bannerView.GetWidthInPixels());
    }

    private void OnBannerAdLoadFailed(LoadAdError error)
    {
        Debug.LogError("Banner view failed to load an ad with error : "
                       + error);
    }

    #endregion

    #endregion
}
