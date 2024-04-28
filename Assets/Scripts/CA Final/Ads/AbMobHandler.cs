using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

namespace GamesForMobileDevices.CA_Final.Ads
{
    public class AbMobHandler : MonoBehaviour, IAdHandler
    {
        private const string BannerAdUnitId = "ca-app-pub-6128683645766691/9008015842";
        private const string InterstitialAdUnitId = "ca-app-pub-6128683645766691/8221214644";
        private const string RewardedAdUnitId = "ca-app-pub-6128683645766691/4289708493";

        private static GameManager GameManager => GameManager.Instance;
        private BannerView bannerView;
        private InterstitialAd interstitialAd;
        private RewardedAd rewardedAd;

        public void Load()
        {
            MobileAds.Initialize((InitializationStatus initStatus) => {});
            RequestConfiguration requestConfiguration = new RequestConfiguration();
            requestConfiguration.TestDeviceIds.Add("780196522C97BACE6CCDCBFF291B0F4A");
            MobileAds.SetRequestConfiguration(requestConfiguration);
        }

        public void LoadAndShowBanner()
        {
            if (bannerView == null)
            {
                bannerView = new BannerView(BannerAdUnitId, AdSize.Banner, AdPosition.Bottom);
            }

            AdRequest adRequest = new AdRequest();
            bannerView.LoadAd(adRequest);
        }

        public void DestroyBanner()
        {
            bannerView?.Destroy();
        }

        public void LoadAndShowInterstitial()
        {
            if (interstitialAd != null)
            {
                interstitialAd.Destroy();
                interstitialAd = null;
            }
            
            AdRequest adRequest = new AdRequest();
            InterstitialAd.Load(InterstitialAdUnitId, adRequest,
                (InterstitialAd ad, LoadAdError error) =>
                {
                    interstitialAd = ad;
                    interstitialAd.Show();
                });
        }

        public void LoadAndShowRewardedVideo()
        {
            if (rewardedAd != null)
            {
                rewardedAd.Destroy();
                rewardedAd = null;
            }
            
            AdRequest adRequest = new AdRequest();
            RewardedAd.Load(RewardedAdUnitId, adRequest,
                (RewardedAd ad, LoadAdError error) =>
                {
                    rewardedAd = ad;
                    rewardedAd.Show((Reward reward) =>
                    {
                        GameManager.AddCoins((int)reward.Amount);
                    });
                });
        }
    }
}