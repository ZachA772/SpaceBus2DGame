using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;//Singleton instance for global access

    public int enemiesDestroyed = 0;//Counter for enemies destroyed
    public int score = 0;//Player score

    private TMP_Text scoreText;//Reference to the on-screen score text

    private void Awake()
    {
        if (Instance == null)//If no instance exists, make this the singleton
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);//Keep this object between scenes
            SceneManager.sceneLoaded += OnSceneLoaded;//Subscribe to scene load event

            //Set consistent framerate
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
        }
        else
        {
            Destroy(gameObject);//Destroy duplicate GameManager instances
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Reset counters when starting Level1
        if (scene.name == "Level1")
        {
            enemiesDestroyed = 0;
            score = 0;
        }

        //Find the score text object in the scene
        scoreText = GameObject.Find("ScoreText")?.GetComponent<TMP_Text>();
        UpdateScoreUI();//Update UI after loading new scene
    }

    private void Start()
    {
        //Add points over time every 1 second
        InvokeRepeating(nameof(AddTimeScore), 1f, 1f);
    }

    private void AddTimeScore()
    {
        score += 10;//Increment score
        UpdateScoreUI();//Update UI
    }

    public void AddKill()
    {
        enemiesDestroyed++;//Increment enemies destroyed
        score += 20;//Add points for kill
        UpdateScoreUI();//Update UI

        Debug.Log("Enemies Destroyed: " + enemiesDestroyed +
             " | Current Scene: " + SceneManager.GetActiveScene().name);//Debug info
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;//Display current score
    }
}
