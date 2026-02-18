using UnityEngine;

public class Boss2Controller : MonoBehaviour
{
    [Header("Vertical Movement")]

    //Speed at which the boss moves up and down
    [SerializeField] private float moveSpeed = 2f;

    //Minimum vertical position limit
    [SerializeField] private float minY = -2f;

    //Maximum vertical position limit
    [SerializeField] private float maxY = 2f;

    [Header("Vision Circle")]

    //Prefab for the vision circle that follows the boss pupil
    [SerializeField] private GameObject visionCirclePrefab;

    //Reference to the pupil transform that the vision circle follows
    [SerializeField] private Transform pupil;

    [Header("Minion Spawning")]

    //Prefab of the minion that Boss 2 spawns
    [SerializeField] private GameObject boss2MinionPrefab;

    //Time interval between minion spawns
    [SerializeField] private float minionSpawnInterval = 3f;

    //Timer used to track time since last minion spawn
    private float minionSpawnTimer = 0f;

    //Reference to the instantiated vision circle
    private GameObject visionCircleInstance;

    //Direction of vertical movement (1 = up, -1 = down)
    private int direction = 1;

    private void Start()
    {
        SpawnVisionCircle();
    }

    private void Update()
    {
        MoveVertically();
        HandleMinionSpawning();
    }

    //Spawns the vision circle and assigns the pupil as its target
    private void SpawnVisionCircle()
    {
        if (visionCirclePrefab == null || pupil == null) return;

        //Instantiate the vision circle prefab
        visionCircleInstance = Instantiate(visionCirclePrefab);

        //Get the follower script from the vision circle
        Boss2VisionCircle follower = visionCircleInstance.GetComponent<Boss2VisionCircle>();

        //Assign the pupil as the target for the vision circle to follow
        if (follower != null)
            follower.SetTarget(pupil);
    }

    //Handles vertical movement between minY and maxY
    private void MoveVertically()
    {
        float newY = transform.position.y + moveSpeed * direction * Time.deltaTime;

        //Reverse direction when reaching vertical limits
        if (newY >= maxY)
        {
            newY = maxY;
            direction = -1;
        }
        else if (newY <= minY)
        {
            newY = minY;
            direction = 1;
        }

        //Apply the new position to the boss
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    //Handles spawning minions at regular intervals
    private void HandleMinionSpawning()
    {
        if (boss2MinionPrefab == null) return;

        //Increment timer by elapsed frame time
        minionSpawnTimer += Time.deltaTime;

        //Check if enough time has passed to spawn a minion
        if (minionSpawnTimer >= minionSpawnInterval)
        {
            minionSpawnTimer = 0f;

            //Generate random X position within range
            float randomX = Random.Range(-12f, 12f);

            //Set spawn position with fixed Y value
            Vector3 spawnPosition = new Vector3(randomX, 6f, 0f);

            //Instantiate the minion at the spawn position
            Instantiate(boss2MinionPrefab, spawnPosition, Quaternion.identity);
        }
    }

    //Called when the boss is destroyed
    private void OnDestroy()
    {
        //Destroy the vision circle if it exists
        if (visionCircleInstance != null)
            Destroy(visionCircleInstance);
    }
}
