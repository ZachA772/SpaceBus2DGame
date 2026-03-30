using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GooglePlayManager : MonoBehaviour
{
    public static GooglePlayManager Instance;

    private bool isAuthenticated = false;

    //achievement
    private bool firstKillUnlocked = false;
    private bool thirtyKillsUnlocked = false;
    private bool firstDeathUnlocked = false;
    private bool firstBossUnlocked = false;
    private bool secondBossUnlocked = false;
    private bool thirdBossUnlocked = false;
    private bool completedGameUnlocked = false;
    

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePlayGames();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePlayGames()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate(success => {
            isAuthenticated = success;
            Debug.Log("Play Games sign in: " + success);
        });
    }

    public void ReportScore(long score)
    {
        if (!isAuthenticated) return;

        Social.ReportScore(score, GPGSIds.leaderboard_highscoreleaderboard, success =>
        {
            Debug.Log("Leaderboard submit: " + success + " | score: " + score);
        });
    }

    public void UnlockAchievement(string achievementId)
    {
        Debug.Log("UnlockAchievement called. auth=" + isAuthenticated + " id=" + achievementId);
        if (!isAuthenticated) return;

        Social.ReportProgress(achievementId, 100.0f, success =>
        {
            Debug.Log("Achievement unlock: " + achievementId + " | " + success);
        });
    }

    public void FirstKill()
    {
        if (!firstKillUnlocked)
        {
            firstKillUnlocked = true;
            UnlockAchievement(GPGSIds.achievement_blow_em_up);
        }
    }

    public void ThirtyKills()
    {
        if (!thirtyKillsUnlocked)
        {
            thirtyKillsUnlocked = true;
            UnlockAchievement(GPGSIds.achievement_cleaning_the_galaxy);
        }
    }

    public void RecordDeath()
    {
        if (firstDeathUnlocked) return;

        firstDeathUnlocked = true;
        UnlockAchievement(GPGSIds.achievement_dont_give_up);
    }

    public void RecordReachFirstBoss()
    {
        if (firstBossUnlocked) return;

        firstBossUnlocked = true;
        UnlockAchievement(GPGSIds.achievement_hand_of_god);
    }

    public void RecordReachSecondBoss()
    {
        if (secondBossUnlocked) return;

        secondBossUnlocked = true;
        UnlockAchievement(GPGSIds.achievement_eye_of_the_maker);
    }

    public void RecordReachThirdBoss()
    {
        if (thirdBossUnlocked) return;

        thirdBossUnlocked = true;
        UnlockAchievement(GPGSIds.achievement_the_creator);
    }

    public void RecordGameCompleted()
    {
        if (completedGameUnlocked) return;

        completedGameUnlocked = true;
        UnlockAchievement(GPGSIds.achievement_saved_the_universe);
    }

    public void ShowAchievementsUI()
    {
        Debug.Log("ShowAchievementsUI called. isAuthenticated = " + isAuthenticated);
        if (!isAuthenticated) return;
        Social.ShowAchievementsUI();
    }

    public void ShowLeaderboardUI()
    {
        Debug.Log("ShowLeaderboardUI called. isAuthenticated = " + isAuthenticated);
        if (!isAuthenticated) return;
        Social.ShowLeaderboardUI();
    }
}