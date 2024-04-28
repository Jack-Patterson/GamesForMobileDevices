using UnityEngine;

namespace GamesForMobileDevices.CA_Final.Ads
{
    public interface IAdHandler
    {
        public abstract void Load();

        public abstract void LoadAndShowBanner();
        public abstract void DestroyBanner();
        public abstract void LoadAndShowInterstitial();
        public abstract void LoadAndShowRewardedVideo();
    }
}