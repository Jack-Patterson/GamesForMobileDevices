﻿using GamesForMobileDevices.CA_Final.Ads;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine;
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

        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text coinsText;
        [SerializeField] private TMP_Text leaderboardText;

        private new void Start()
        {
            base.Start();
            AdManager.Instance.LoadAndShowBanner();
            SubmitScoreToLeaderboard(100);
            LoadTopScores();
        }

        protected override void AssignButtons()
        {
            playButton.onClick.AddListener(() =>
            {
                PlayGamesHandler.instance.UnlockAchievement(GPGSIds.achievement_press_play);
                AdManager.Instance.LoadAndShowInterstitial();
                GameManager.SwitchToScene("CATemplateMenu");
            });
            achievementsButton.onClick.AddListener(() => GameManager.SwitchToScene("Achievements"));
            storeButton.onClick.AddListener(() => { GameManager.SwitchToScene("Store"); });
            watchAdButton.onClick.AddListener(() => AdManager.Instance.LoadAndShowRewardedVideo());
        }

        protected override void SubscribeText()
        {
            GameManager.OnLevelChanged += level => { levelText.text = $"Level: {level}"; };

            GameManager.OnCoinsChanged += coins => { coinsText.text = $"Coins: {coins}"; };
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
        
        public void SubmitScoreToLeaderboard(int score)
        {
            PlayGamesPlatform.Instance.ReportScore(score, GPGSIds.leaderboard_highest_level, (bool success) => {
                if (success)
                {
                    Debug.Log("Score submitted successfully!");
                }
                else
                {
                    Debug.Log("Failed to submit score.");
                }
            });
        }
    }
}