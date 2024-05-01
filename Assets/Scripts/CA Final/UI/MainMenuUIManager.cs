using GamesForMobileDevices.CA_Final.Ads;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

namespace GamesForMobileDevices.CA_Final.UI
{
    public class MainMenuUIManager : UIManager
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button achievementsButton;
        [SerializeField] private Button storeButton;
        [SerializeField] private Button watchAdButton;
        [SerializeField] private Button twitterButton;

        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text coinsText;
        [SerializeField] private TMP_Text leaderboardText;

        private new void Start()
        {
            base.Start();
            AdManager.Instance.LoadAndShowBanner();
            LoadTopScores();
        }

        protected override void AssignButtons()
        {
            playButton.onClick.AddListener(() =>
            {
                PlayGamesHandler.instance.UnlockAchievement(GPGSIds.achievement_press_play);
                AdManager.Instance.LoadAndShowInterstitial();
                GameManager.SwitchToScene("Game");
            });
            achievementsButton.onClick.AddListener(() => PlayGamesHandler.instance.ShowAchievements());
            storeButton.onClick.AddListener(() => { GameManager.SwitchToScene("Store"); });
            watchAdButton.onClick.AddListener(() => AdManager.Instance.LoadAndShowRewardedVideo());
            twitterButton.onClick.AddListener(ShareScoreTwitter);
        }

        protected override void SubscribeText()
        {
            GameManager.onLevelChanged += level => { levelText.text = $"Level: {level}"; };

            GameManager.onCoinsChanged += coins => { coinsText.text = $"Coins: {coins}"; };
        }

        protected override void AssignInitialText()
        {
            levelText.text = $"Level: {GameManager.CurrentLevel}";
            coinsText.text = $"Coins: {GameManager.Coins}";
        }

        private void LoadTopScores()
        {
            PlayGamesPlatform.Instance.LoadScores(
                GPGSIds.leaderboard_highest_level, LeaderboardStart.TopScores, 5, LeaderboardCollection.Public,
                LeaderboardTimeSpan.AllTime, (data) =>
                {
                    if (data.Valid)
                    {
                        DisplayScores(data.Scores);
                    }
                    else
                    {
                        leaderboardText.text = "Failed to load leaderboard.";
                    }
                }
            );
        }

        private void DisplayScores(IScore[] scores)
        {
            string formattedScores = "";
            foreach (IScore score in scores)
            {
                formattedScores += $"{score.rank}. {score.userID} - {score.value}\n";
            }

            leaderboardText.text = formattedScores;
        }
        
        
        
        private void ShareScoreTwitter()
        {
            string twitterMessage = $"I've got {GameManager.CurrentLevel} levels in this cool game!";
            string url = $"https://twitter.com/intent/tweet?text={UnityWebRequest.EscapeURL(twitterMessage)}";
            
            Application.OpenURL(url);
        }
    }
}