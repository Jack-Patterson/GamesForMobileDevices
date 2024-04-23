using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.GamesForMobileDevices.CA_Final.UI
{
    public class MainMenuUIManager : UIManager
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button achievementsButton;
        [SerializeField] private Button storeButton;
        [SerializeField] private Button watchAdButton;

        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text coinsText;

        protected override void AssignButtons()
        {
            playButton.onClick.AddListener(() =>
            {
                IronSource.Agent.loadInterstitial();
                GameManager.SwitchToScene("CATemplateMenu");
            });
            achievementsButton.onClick.AddListener(() => GameManager.SwitchToScene("Achievements"));
            storeButton.onClick.AddListener(() => GameManager.IncreaseLevel()); //GameManager.SwitchToScene("Store"));
            watchAdButton.onClick.AddListener(() => IronSource.Agent.showRewardedVideo());
        }

        protected override void SubscribeText()
        {
            GameManager.onLevelChanged += level =>
            {
                levelText.text = $"Level: {level}";
            };
            
            GameManager.onCoinsChanged += coins =>
            {
                coinsText.text = $"Coins: {coins}";
            };
        }
        
        protected override void AssignInitialText()
        {
            levelText.text = $"Level: {GameManager.CurrentLevel}";
            coinsText.text = $"Coins: {GameManager.Coins}";
        }
    }
}