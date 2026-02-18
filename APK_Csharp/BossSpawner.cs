using System.Collections;
using TMPro;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [Header("Boss 1 Settings")]
    //Prefab for Boss 1
    [SerializeField] private GameObject boss1Prefab;
    //Position where Boss 1 will spawn
    [SerializeField] private Transform boss1SpawnPoint;
    //Number of enemies the player must destroy before Boss 1 appears
    [SerializeField] private int boss1KillsRequired = 5;

    [Header("Boss 2 Settings")]
    //Prefab for Boss 2
    [SerializeField] private GameObject boss2Prefab;
    //Position where Boss 2 will spawn
    [SerializeField] private Transform boss2SpawnPoint;
    //Number of enemies required to spawn Boss 2
    [SerializeField] private int boss2KillsRequired = 5;

    [Header("Boss 3 Settings")]
    //Prefab for Boss 3
    [SerializeField] private GameObject boss3Prefab;
    //Position where Boss 3 will spawn
    [SerializeField] private Transform boss3SpawnPoint;
    //Number of enemies required to spawn Boss 3
    [SerializeField] private int boss3KillsRequired = 5;

    //Reference to the boss UI container
    [SerializeField] private GameObject BossUI;
    //Text element that displays the boss name
    [SerializeField] private TMP_Text BossNameText;
    //Reference to the normal enemy spawner so it can be stopped and resumed
    [SerializeField] private EnemySpawner enemySpawner;

    //Tracks whether a boss is currently active
    private bool bossSpawned = false;
    //Tracks whether the boss has already been defeated
    private bool bossDefeated = false;

    //Reference to the level complete text object
    [SerializeField] private GameObject levelCompleteText;

    private void Update()
    {
        //Do nothing if a boss is already active or defeated
        if (bossSpawned || bossDefeated) return;

        //Check if enough enemies are destroyed in Level 1
        if (GameManager.Instance.enemiesDestroyed == boss1KillsRequired &&
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Level1")
        {
            SpawnBoss1();
        }

        //Check if enough enemies are destroyed in Level 2
        if (GameManager.Instance.enemiesDestroyed == boss2KillsRequired &&
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Level2")
        {
            SpawnBoss2();
        }

        //Check if enough enemies are destroyed in Level 3
        if (GameManager.Instance.enemiesDestroyed == boss3KillsRequired &&
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Level3")
        {
            SpawnBoss3();
        }
    }

    private void SpawnBoss1()
    {
        //Mark boss as active
        bossSpawned = true;

        //Create the boss at its spawn position
        GameObject boss = Instantiate(boss1Prefab, boss1SpawnPoint.position, Quaternion.identity);

        //Show boss UI and set boss name
        if (BossUI != null)
        {
            BossUI.SetActive(true);
            if (BossNameText != null)
                BossNameText.text = "Hand of God";
        }

        //Get the BossHealth component to detect when the boss dies
        BossHealth bh = boss.GetComponent<BossHealth>();
        if (bh != null)
        {
            bh.OnBossDeath += () =>
            {
                //Hide boss UI when boss dies
                if (BossUI != null)
                    BossUI.SetActive(false);

                //Resume normal enemy spawning
                if (enemySpawner != null)
                    enemySpawner.ResumeSpawning();

                //Update boss state flags
                bossSpawned = false;
                bossDefeated = true;

                //Show level complete message
                if (levelCompleteText != null)
                {
                    TMP_Text TextLevelComplete = levelCompleteText.GetComponent<TMP_Text>();
                    TextLevelComplete.text = "Level 1 Complete";
                    levelCompleteText.SetActive(true);
                }

                //Load next level after delay
                StartCoroutine(LoadNextSceneAfterDelay("Level2", 3f));
            };
        }

        //Stop normal enemy spawning while boss is active
        if (enemySpawner != null)
            enemySpawner.StopSpawning();

        Debug.Log("Boss 1 spawned!");
    }

    private void SpawnBoss2()
    {
        //Mark boss as active
        bossSpawned = true;

        //Create Boss 2
        GameObject boss = Instantiate(boss2Prefab, boss2SpawnPoint.position, Quaternion.identity);

        //Show boss UI and set boss name
        if (BossUI != null)
        {
            BossUI.SetActive(true);
            if (BossNameText != null)
                BossNameText.text = "Eye of The Maker";
        }

        //Listen for boss death event
        BossHealth bh = boss.GetComponent<BossHealth>();
        if (bh != null)
        {
            bh.OnBossDeath += () =>
            {
                //Hide boss UI
                if (BossUI != null)
                    BossUI.SetActive(false);

                //Resume enemy spawning
                if (enemySpawner != null)
                    enemySpawner.ResumeSpawning();

                //Update boss state flags
                bossSpawned = false;
                bossDefeated = true;

                //Show level complete message
                if (levelCompleteText != null)
                {
                    TMP_Text TextLevelComplete = levelCompleteText.GetComponent<TMP_Text>();
                    TextLevelComplete.text = "Level 2 Complete";
                    levelCompleteText.SetActive(true);
                }

                //Load next level
                StartCoroutine(LoadNextSceneAfterDelay("Level3", 3f));
            };
        }

        //Stop enemy spawning during boss fight
        if (enemySpawner != null)
            enemySpawner.StopSpawning();

        Debug.Log("Boss 2 spawned!");
    }

    private void SpawnBoss3()
    {
        //Mark boss as active
        bossSpawned = true;

        //Create Boss 3
        GameObject boss = Instantiate(boss3Prefab, boss3SpawnPoint.position, Quaternion.identity);

        //Show boss UI and set boss name
        if (BossUI != null)
        {
            BossUI.SetActive(true);
            if (BossNameText != null)
                BossNameText.text = "The Creator";
        }

        //Listen for boss death event
        BossHealth bh = boss.GetComponent<BossHealth>();
        if (bh != null)
        {
            bh.OnBossDeath += () =>
            {
                //Hide boss UI
                if (BossUI != null)
                    BossUI.SetActive(false);

                //Resume enemy spawning
                if (enemySpawner != null)
                    enemySpawner.ResumeSpawning();

                //Update boss state flags
                bossSpawned = false;
                bossDefeated = true;

                //Show final completion message
                if (levelCompleteText != null)
                {
                    TMP_Text TextLevelComplete = levelCompleteText.GetComponent<TMP_Text>();
                    TextLevelComplete.text = "The Creator is Defeated";
                    levelCompleteText.SetActive(true);
                }

                //Load win screen
                StartCoroutine(LoadNextSceneAfterDelay("WinScreen", 3f));
            };
        }

        //Stop enemy spawning during boss fight
        if (enemySpawner != null)
            enemySpawner.StopSpawning();

        Debug.Log("Boss 3 spawned!");
    }

    private IEnumerator LoadNextSceneAfterDelay(string sceneName, float delay)
    {
        //Wait for the specified delay
        yield return new WaitForSeconds(delay);

        //Load the specified scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
