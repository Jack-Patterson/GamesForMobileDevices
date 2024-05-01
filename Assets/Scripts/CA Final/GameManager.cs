using System;
using GamesForMobileDevices.CA_Final.Ads;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamesForMobileDevices.CA_Final
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public int CurrentLevel { get; private set; } = 1;
        public int Coins { get; private set; }
        private bool _areAdsEnabled = true;
        private bool _useDefaultPlayerModel = true;
        
        private bool _isGameOver = false;
        public bool IsGameOver
        {
            get => _isGameOver;
            set => _isGameOver = value;
        }

        public bool AreAdsEnabled
        {
            get => _areAdsEnabled;
            set
            {
                _areAdsEnabled = value;
                if (_areAdsEnabled)
                {
                    AdManager.Instance.LoadAndShowBanner();
                }
                else
                {
                    AdManager.Instance.DestroyBanners();
                }
            }
        }
        public bool UseDefaultPlayerModel
        {
            get => _useDefaultPlayerModel;
            set => _useDefaultPlayerModel = value;
        }

        public Action<int> onLevelChanged;
        public Action<int> onCoinsChanged;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            PlayGamesHandler.instance.UnlockAchievement(GPGSIds.achievement_log_in);
        }

        public void IncreaseLevel()
        {
            CurrentLevel++;
            onLevelChanged?.Invoke(CurrentLevel);
        }

        public void AddCoins(int amount)
        {
            Coins += amount;
            onCoinsChanged?.Invoke(Coins);
        }

        public void SwitchToScene(string sceneName)
        {
            AdManager.Instance?.DestroyBanners();
            SceneManager.LoadScene(sceneName);
            AdManager.Instance?.LoadAndShowBanner();
        }
    }
}