using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.GamesForMobileDevices.CA_Final
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public int CurrentLevel { get; private set; } = 1;
        public int Coins { get; private set; }
        
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
            SceneManager.LoadScene(sceneName);
            IronSource.Agent.destroyBanner();
        }
    }
}