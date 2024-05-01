using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GamesForMobileDevices.CA_Final.UI
{
    public class GameUIManager : UIManager
    {
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuGoButton;
        [SerializeField] private GameObject levelWonPanel;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button mainMenuLwButton;
        [SerializeField] private TMP_Text levelText;

        private new void Start()
        {
            base.Start();
            gameOverPanel.SetActive(false);
            levelWonPanel.SetActive(false);
        }

        protected override void AssignButtons()
        {
            restartButton.onClick.AddListener(() =>
            {
                GameManager.SwitchToScene("Game");
                GameManager.IsGameOver = false;
            });
            mainMenuGoButton.onClick.AddListener(() =>
            {
                GameManager.SwitchToScene("MainMenu");
                GameManager.IsGameOver = false;
            });

            nextLevelButton.onClick.AddListener(() =>
            {
                GameManager.IncreaseLevel();
                PlayGamesHandler.instance.SubmitScoreToLeaderboard(GameManager.CurrentLevel);
                GameManager.SwitchToScene("Game");
                GameManager.IsGameOver = false;
            });
            mainMenuLwButton.onClick.AddListener(() =>
            {
                GameManager.SwitchToScene("MainMenu");
                GameManager.IsGameOver = false;
            });
        }

        protected override void SubscribeText()
        {
            levelText.text = $"Level: {GameManager.CurrentLevel.ToString()}";
        }

        public void ShowGameOver()
        {
            gameOverPanel.SetActive(true);
        }

        public void ShowLevelWon()
        {
            levelWonPanel.SetActive(true);
        }
    }
}