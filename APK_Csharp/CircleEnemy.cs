using UnityEngine;

public class CircleEnemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;//Speed at which the enemy moves toward the player

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;//Bullet prefab to spawn when shooting
    [SerializeField] private float shootInterval = 2f;//Time between each shooting cycle
    [SerializeField] private float bulletSpeed = 6f;//Speed of each bullet
    [SerializeField] private int bulletCount = 8;//Number of bullets fired in a full circle

    [Header("Death")]
    [SerializeField] private float destroyDelay = 0.3f;//Delay before enemy is destroyed after death

    private Animator animator;//Controls enemy animations
    private Rigidbody2D rb;//Controls enemy physics
    private Transform player;//Reference to the player's transform
    private bool isDestroyed = false;//Prevents logic from running after enemy is destroyed

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;//AudioSource component for playing sounds
    [SerializeField] private AudioClip deathSound;//Sound played when enemy dies
    [SerializeField] private AudioClip shootSound;//Sound played when enemy shoots

    private void Start()
    {
        //Find the player object using its tag and store its transform
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        //Get required components
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        //Call ShootCircle repeatedly after shootInterval seconds
        InvokeRepeating(nameof(ShootCircle), shootInterval, shootInterval);
    }

    private void Update()
    {
        //Stop movement if player doesn't exist or enemy is destroyed
        if (player == null || isDestroyed) return;

        //Calculate direction from enemy to player
        Vector2 toPlayer = (player.position - transform.position).normalized;

        //Move enemy toward player
        transform.position += (Vector3)(toPlayer * moveSpeed * Time.deltaTime);
    }

    private void ShootCircle()
    {
        //Do nothing if enemy is destroyed
        if (isDestroyed) return;

        //Play shooting sound
        if (audioSource != null && shootSound != null)
            audioSource.PlayOneShot(shootSound);

        //Calculate angle between each bullet
        float angleStep = 360f / bulletCount;

        //Spawn bullets in a circular pattern
        for (int i = 0; i < bulletCount; i++)
        {
            //Calculate angle for this bullet
            float angle = angleStep * i;

            //Rotate upward direction by the angle to get circular spread
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;

            //Create the bullet at enemy position
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            //Set bullet velocity
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = direction * bulletSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Ignore if already destroyed
        if (isDestroyed) return;

        //Check if hit by player bullet
        if (other.CompareTag("Bullet"))
        {
            isDestroyed = true;//Mark enemy as destroyed to stop logic

            CancelInvoke();//Stop ShootCircle from being called again

            //Stop enemy movement
            if (rb != null)
                rb.velocity = Vector2.zero;

            //Trigger death animation
            if (animator != null)
                animator.SetTrigger("OnDeath");

            //Play death sound
            if (audioSource != null && deathSound != null)
                audioSource.PlayOneShot(deathSound);

            Destroy(other.gameObject);//Destroy the bullet

            GameManager.Instance.AddKill();//Increase player kill count

            Destroy(gameObject, destroyDelay);//Destroy enemy after delay
        }
    }
}
