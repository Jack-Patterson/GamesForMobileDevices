using UnityEngine;

namespace com.GamesForMobileDevices.CA_Final.Ads
{
    public class LevelPlayHandler : MonoBehaviour
    {
        private void OnEnable()
        {
            IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;

            IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
            IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
            IronSourceBannerEvents.onAdClickedEvent += BannerOnAdClickedEvent;
            IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
            IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
            IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;
            IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        }

        private void Start()
        {
            IronSource.Agent.validateIntegration();
            // IronSource.Agent.init("85460dcd");
            IronSource.Agent.init("1e45e7f1d");
        }

        void OnApplicationPause(bool isPaused)
        {
            IronSource.Agent.onApplicationPause(isPaused);
        }

        private void SdkInitializationCompletedEvent()
        {
            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
            print("SDK Initialization completed - Start loading ads");
        }
        
        void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo)
        {
            IronSource.Agent.showInterstitial();
        }
        
        void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo){
            GameManager.instance.AddCoins(20);
        }

        #region Banner

        //Invoked once the banner has loaded
        void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo)
        {
        }

//Invoked when the banner loading process has failed.
        void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError)
        {
        }

// Invoked when end user clicks on the banner ad
        void BannerOnAdClickedEvent(IronSourceAdInfo adInfo)
        {
        }

//Notifies the presentation of a full screen content following user click
        void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo)
        {
        }

//Notifies the presented screen has been dismissed
        void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo)
        {
        }

//Invoked when the user leaves the app
        void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo)
        {
        }

        #endregion
    }
}