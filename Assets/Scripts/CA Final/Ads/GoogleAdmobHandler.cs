using GoogleMobileAds.Api;
using UnityEngine;

namespace com.GamesForMobileDevices.CA_Final.Ads
{
    public class GoogleAdmobHandler : MonoBehaviour
    {
#if UNITY_ANDROID
        private string _adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
  private string _adUnitId = "unused";
#endif

        private BannerView _bannerView;


        private void Start()
        {
            MobileAds.Initialize(status => { CreateBannerView(); });
        }

        public void CreateBannerView()
        {
            Debug.Log("Creating banner view");

            if (_bannerView != null)
            {
                _bannerView.Destroy();
            }

            // Create a 320x50 banner at top of the screen
            _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Top);
        }
    }
}