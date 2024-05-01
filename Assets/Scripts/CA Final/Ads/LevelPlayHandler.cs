using UnityEngine;

namespace GamesForMobileDevices.CA_Final.Ads
{
    public class LevelPlayHandler : MonoBehaviour, IAdHandler
    {
        private static GameManager GameManager => GameManager.instance;

        public void Load()
        {
            IronSource.Agent.validateIntegration();
            IronSource.Agent.init("1e45e7f1d");

            IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        }

        public void LoadAndShowBanner()
        {
            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
        }

        public void DestroyBanner()
        {
            IronSource.Agent.destroyBanner();
        }

        public void LoadAndShowInterstitial()
        {
            IronSource.Agent.loadInterstitial();
        }

        public void LoadAndShowRewardedVideo()
        {
            IronSource.Agent.showRewardedVideo();
        }

        void OnApplicationPause(bool isPaused)
        {
            IronSource.Agent.onApplicationPause(isPaused);
        }

        void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo)
        {
            IronSource.Agent.showInterstitial();
        }

        void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
        {
            GameManager.AddCoins(20);
            PlayGamesHandler.instance.UnlockAchievement(GPGSIds.achievement_watch_a_rewarded_ad);
        }
    }
}