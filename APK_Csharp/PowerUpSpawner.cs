using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("PowerUp Prefabs")]
    [SerializeField] private GameObject homingBulletsPrefab;//Prefab for homing bullets power-up
    [SerializeField] private GameObject shieldPrefab;//Prefab for shield power-up
    [SerializeField] private GameObject multiShotPrefab;//Prefab for multi-shot power-up

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 8f;//Time between spawns
    [SerializeField] private Transform MinYSpawn;//Min Y
    [SerializeField] private Transform MaxYSpawn;//Max Y

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 4f;//Speed at which power-ups move left

    private void Start()
    {
        InvokeRepeating(nameof(SpawnRandomPowerUp), 3f, spawnInterval);//Spawn power-ups repeatedly
    }

    private void SpawnRandomPowerUp()
    {
        //Choose random prefab
        int randomIndex = Random.Range(0, 3);
        GameObject prefabToSpawn = null;

        switch (randomIndex)
        {
            case 0:
                prefabToSpawn = homingBulletsPrefab;
                break;
            case 1:
                prefabToSpawn = shieldPrefab;
                break;
            case 2:
                prefabToSpawn = multiShotPrefab;
                break;
        }

        if (prefabToSpawn == null) return;//No prefab assigned, exit

        //Set spawn position with random Y
        Vector3 spawnPos = transform.position;
        spawnPos.y = Random.Range(MinYSpawn.position.y, MaxYSpawn.position.y);

        GameObject powerUp = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);//Spawn power-up

        //Rigidbody2D for movement
        Rigidbody2D rb = powerUp.GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = powerUp.AddComponent<Rigidbody2D>();

        rb.gravityScale = 0;//Prevent falling
        rb.velocity = Vector2.left * moveSpeed;//Move left
    }
}
