using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

namespace GamesForMobileDevices.CA_Final
{
    public class PlayGamesHandler : MonoBehaviour
    {
        public static PlayGamesHandler instance;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            #if UNITY_ANDROID && !UNITY_EDITOR
            PlayGamesPlatform.Activate();
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
            #endif
        }

        private void ProcessAuthentication(SignInStatus signInStatus)
        {
            if (signInStatus == SignInStatus.Success)
            {
                Debug.Log("Login with Google Play games successful.");
            }
            else
            {
                Debug.Log("Login Unsuccessful");
                PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
            }
        }
        
        public void UnlockAchievement(string achievementId)
        {
            Social.ReportProgress(achievementId, 100.0f, success =>
            {
                Debug.Log(success ? "Achievement unlocked" : "Achievement failed");
            });
        }
        
        public void ShowAchievements()
        {
            Social.ShowAchievementsUI();
        }
    }
}