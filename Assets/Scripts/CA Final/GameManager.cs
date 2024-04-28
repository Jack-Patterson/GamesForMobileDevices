using System;
using GamesForMobileDevices.CA_Final.Ads;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamesForMobileDevices.CA_Final
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public int CurrentLevel { get; private set; } = 1;
        public int Coins { get; private set; }
        private bool areAdsEnabled = true;

        public bool AreAdsEnabled
        {
            get => areAdsEnabled;
            set
            {
                areAdsEnabled = value;
                if (areAdsEnabled)
                {
                    AdManager.Instance.LoadAndShowBanner();
                }
                else
                {
                    AdManager.Instance.DestroyBanners();
                }
            }
        }

        public Action<int> OnLevelChanged;
        public Action<int> OnCoinsChanged;

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
            PlayGamesHandler.instance.UnlockAchievement(GPGSIds.achievement_log_in);
        }

        public void IncreaseLevel()
        {
            CurrentLevel++;
            OnLevelChanged?.Invoke(CurrentLevel);
        }

        public void AddCoins(int amount)
        {
            Coins += amount;
            OnCoinsChanged?.Invoke(Coins);
        }

        public void SwitchToScene(string sceneName)
        {
            AdManager.Instance.DestroyBanners();
            SceneManager.LoadScene(sceneName);
            AdManager.Instance.LoadAndShowBanner();
        }
    }
}