using UnityEngine;

public class Boss1Controller : MonoBehaviour
{
    [Header("Vertical Movement")]

    //Speed at which the boss moves up and down
    [SerializeField] private float moveSpeed = 2f;

    //Minimum vertical position
    [SerializeField] private float minY = -2f;

    //Maximum vertical position
    [SerializeField] private float maxY = 2f;

    [Header("Shooting")]

    //Prefab of the bullet the boss will shoot
    [SerializeField] private GameObject bulletPrefab;

    //Empty transform indicating where bullets spawn
    [SerializeField] private Transform shootPoint;

    //Speed at which the bullets travel
    [SerializeField] private float bulletSpeed = 10f;

    //Time interval between consecutive shots
    [SerializeField] private float shootInterval = 4f;

    [Header("Audio")]

    //Audio source for boss sounds
    [SerializeField] private AudioSource audioSource;

    //Audio clip to play when shooting
    [SerializeField] private AudioClip shootSound;

    //1 = moving up, -1 = moving down
    private int direction = 1;

    //Timer to track time since last shot
    private float shootTimer = 0f;

    //Animator component for boss animations
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        MoveVertically();
        HandleShooting();
    }

    //Handles vertical movement between minY and maxY
    private void MoveVertically()
    {
        float newY = transform.position.y + moveSpeed * direction * Time.deltaTime;

        //Reverse direction if hitting top or bottom bounds
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

        //Apply new position
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    //Handles shooting bullets at regular intervals
    private void HandleShooting()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        //Increment timer by time elapsed this frame
        shootTimer += Time.deltaTime;

        //Check if it's time to shoot
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;

            //Trigger shooting animation
            animator.SetTrigger("BossShoot");

            //Play shooting sound
            if (audioSource != null && shootSound != null)
                audioSource.PlayOneShot(shootSound);

            //Instantiate bullet at shootPoint
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

            //Set bullet velocity to move left
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = Vector2.left * bulletSpeed;
        }
    }
}
