using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [Header("Black Hole Variables")]
    [SerializeField] private GameObject blackHolePrefab;//Prefab for black hole enemy
    [SerializeField] private float blackHoleSpawnInterval = 5f;//Time between spawns
    [SerializeField] private float blackHoleSpeed = 5f;//Movement speed of black hole
    [SerializeField] private Transform blackHoleSpawnPoint;//Spawn point transform

    [Header("Asteroid Variable")]
    [SerializeField] private GameObject asteroidPrefab;//Prefab for asteroid enemy
    [SerializeField] private float asteroidSpawnInterval = 3f;//Time between spawns
    [SerializeField] private float asteroidSpeed = 4f;//Movement speed of asteroid
    [SerializeField] private Transform asteroidSpawnPoint;//Spawn point transform


    [Header("Shield Enemy Variables")]
    [SerializeField] private GameObject shieldEnemyPrefab;//Prefab for shield enemy
    [SerializeField] private float shieldEnemySpawnInterval = 5f;//Spawn interval
    [SerializeField] private Transform shieldEnemySpawnPoint;//Spawn point

    [Header("Circle Enemy Variables")]
    [SerializeField] private GameObject circleEnemyPrefab;//Prefab for circle enemy
    [SerializeField] private float circleEnemySpawnInterval = 5f;//Spawn interval
    [SerializeField] private Transform circleEnemySpawnPoint;//Spawn point
    
    [Header("Strafe Enemy Variables")]
    [SerializeField] private GameObject strafeEnemyPrefab;//Prefab for strafe enemy
    [SerializeField] private float strafeEnemySpawnInterval = 5f;//Spawn interval
    [SerializeField] private Transform strafeEnemySpawnPoint;//Spawn point
   
    [Header("Split Enemy Variables")]
    [SerializeField] private GameObject splitEnemyPrefab;//Prefab for split enemy
    [SerializeField] private float splitEnemySpawnInterval = 5f;//Spawn interval
    [SerializeField] private Transform splitEnemySpawnPoint;//Spawn point

    //Max and Min Y spawns
    [SerializeField] private Transform MinYSpawn;//Min Y
    [SerializeField] private Transform MaxYSpawn;//Max Y
    [SerializeField] private Transform MaxYLeftSpawn;//Max Y Top Left



    private void Start()
    {
        ResumeSpawning();//Start spawning enemies when scene starts
    }

    public void ResumeSpawning()
    {
        enabled = true;//Enable this spawner

        string currentScene = SceneManager.GetActiveScene().name;//Get current scene

        //Spawn different enemies based on current level
        if (currentScene == "Level1")
        {
            InvokeRepeating(nameof(SpawnAsteroid), 1f, asteroidSpawnInterval);//Spawn asteroids 
            InvokeRepeating(nameof(SpawnBlackHole), 0f, blackHoleSpawnInterval);//Spawn black holes
        }
        else if (currentScene == "Level2")
        {
            InvokeRepeating(nameof(SpawnCircleEnemy), 1f, circleEnemySpawnInterval);//Spawn circle enemies
            InvokeRepeating(nameof(SpawnStrafeEnemy), 0f, strafeEnemySpawnInterval);//Spawn strafe enemies
            InvokeRepeating(nameof(SpawnAsteroid), 5f, asteroidSpawnInterval);//Spawn asteroids
        }
        else if (currentScene == "Level3")
        {
            InvokeRepeating(nameof(SpawnShieldEnemy), 1f, shieldEnemySpawnInterval);//Spawn shield enemies
            InvokeRepeating(nameof(SpawnSplitEnemy), 0f, splitEnemySpawnInterval);//Spawn split enemies
            InvokeRepeating(nameof(SpawnBlackHole), 0f, blackHoleSpawnInterval);//Spawn black holes
        }
    }

    private void SpawnBlackHole()
    {
        if (blackHolePrefab == null) return;//Return if prefab missing

        Vector3 spawnPos = blackHoleSpawnPoint != null ? blackHoleSpawnPoint.position : transform.position;//Get spawn position
        spawnPos.y = Random.Range(MinYSpawn.position.y, MaxYSpawn.position.y);//Randomize Y

        GameObject blackHole = Instantiate(blackHolePrefab, spawnPos, Quaternion.identity);//Spawn black hole

        Rigidbody2D rb = blackHole.GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = blackHole.AddComponent<Rigidbody2D>();//Add Rigidbody if missing

        rb.gravityScale = 0;//No gravity
        rb.velocity = Vector2.left * blackHoleSpeed;//Move left
    }

    private void SpawnAsteroid()
    {
        if (asteroidPrefab == null) return;//Return if prefab missing

        float randomX = Random.Range(MaxYLeftSpawn.position.x, MaxYSpawn.position.x);//Random X
        Vector3 spawnPos = new Vector3(randomX, asteroidSpawnPoint.position.y, 0f);//Spawn above screen

        GameObject asteroid = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = asteroid.AddComponent<Rigidbody2D>();

        rb.gravityScale = 0;

        Vector2 downLeft = new Vector2(-1f, -1f).normalized;//Direction diagonally down-left
        rb.velocity = downLeft * asteroidSpeed;
    }

    private void SpawnShieldEnemy()
    {
        if (shieldEnemyPrefab == null) return;

        Vector3 spawnPos = shieldEnemySpawnPoint != null ? shieldEnemySpawnPoint.position : transform.position;
        spawnPos.y = Random.Range(MinYSpawn.position.y, MaxYSpawn.position.y);

        GameObject shieldEnemy = Instantiate(shieldEnemyPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = shieldEnemy.GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = shieldEnemy.AddComponent<Rigidbody2D>();

        rb.gravityScale = 0;
    }

    private void SpawnCircleEnemy()
    {
        if (circleEnemyPrefab == null) return;

        Vector3 spawnPos = circleEnemySpawnPoint != null ? circleEnemySpawnPoint.position : transform.position;
        spawnPos.y = Random.Range(MinYSpawn.position.y, MaxYSpawn.position.y);

        GameObject circleEnemy = Instantiate(circleEnemyPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = circleEnemy.GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = circleEnemy.AddComponent<Rigidbody2D>();

        rb.gravityScale = 0;
    }

    private void SpawnStrafeEnemy()
    {
        if (strafeEnemyPrefab == null) return;

        Vector3 spawnPos = strafeEnemySpawnPoint != null ? strafeEnemySpawnPoint.position : transform.position;
        spawnPos.y = Random.Range(MinYSpawn.position.y, MaxYSpawn.position.y);

        GameObject strafeEnemy = Instantiate(strafeEnemyPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = strafeEnemy.GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = strafeEnemy.AddComponent<Rigidbody2D>();

        rb.gravityScale = 0;
    }

    private void SpawnSplitEnemy()
    {
        if (splitEnemyPrefab == null) return;

        Vector3 spawnPos = splitEnemySpawnPoint != null ? splitEnemySpawnPoint.position : transform.position;
        spawnPos.y = Random.Range(MinYSpawn.position.y, MaxYSpawn.position.y);

        GameObject splitEnemy = Instantiate(splitEnemyPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = splitEnemy.GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = splitEnemy.AddComponent<Rigidbody2D>();

        rb.gravityScale = 0;
    }

    public void StopSpawning()
    {
        CancelInvoke();//Stop all repeated invokes
        StopAllCoroutines();//Stop any coroutines
        enabled = false;//Disable this spawner
    }
}
