using UnityEngine;

namespace GamesForMobileDevices.CA_Final.Ads
{
    public class AdManager : MonoBehaviour
    {
        public static AdManager Instance;
        private IAdHandler adMobHandler;
        private IAdHandler levelPlayHandler;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
        
        private void OnEnable()
        {
            adMobHandler = gameObject.AddComponent<AbMobHandler>();
            levelPlayHandler = gameObject.AddComponent<LevelPlayHandler>();
            
            adMobHandler.Load();
            levelPlayHandler.Load();
        }

        public void LoadAndShowBanner()
        {
            if (!GameManager.Instance.AreAdsEnabled)
                return;
            
            if (ShouldUseAdMob())
                adMobHandler.LoadAndShowBanner();
            else
                levelPlayHandler.LoadAndShowBanner();
        }

        public void DestroyBanners()
        {
            adMobHandler.DestroyBanner();
            levelPlayHandler.DestroyBanner();
        }

        public void LoadAndShowInterstitial()
        {
            if (!GameManager.Instance.AreAdsEnabled)
                return;
            
            if (ShouldUseAdMob())
                adMobHandler.LoadAndShowInterstitial();
            else
                levelPlayHandler.LoadAndShowInterstitial();
        }

        public void LoadAndShowRewardedVideo()
        {
            if (ShouldUseAdMob())
                adMobHandler.LoadAndShowRewardedVideo();
            else
                levelPlayHandler.LoadAndShowRewardedVideo();
        }
        
        private bool ShouldUseAdMob() => Random.Range(0f, 1f) < 0.5f;
    }
}