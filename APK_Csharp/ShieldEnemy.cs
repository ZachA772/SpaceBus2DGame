using UnityEngine;

public class ShieldEnemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;//Speed at which enemy moves toward the player

    private Animator animator;//Handles enemy animations
    private Rigidbody2D rb;//Rigidbody for physics/movement
    private bool isDestroyed = false;//Tracks if enemy has been destroyed

    [SerializeField] private float destroyDelay = 0.3f;//Delay before destroying enemy after death animation

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;//Audio source for playing sounds
    [SerializeField] private AudioClip deathSound;//Sound played when enemy dies

    private Transform player;//Reference to player for movement
    private PlayerBulletCleanUp projectileCleanUp;//Reference to check if bullet hit is shielded

    private void Start()
    {
        //Find player and components
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (player == null) return;

        //Move enemy toward player
        Vector2 toPlayer = (player.position - transform.position).normalized;
        transform.position += (Vector3)(toPlayer * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Get reference to the bullet's clean-up script
        projectileCleanUp = GameObject.FindWithTag("Bullet").GetComponent<PlayerBulletCleanUp>();
        if (isDestroyed) return;

        if (other.CompareTag("Bullet"))
        {
            //Check if bullet was a shield shot
            if (projectileCleanUp.GetShieldShot())
            {
                Debug.Log(projectileCleanUp.GetShieldShot());
                projectileCleanUp.SetShieldShot();//Reset flag
                return;//Ignore damage
            }
            else
            {
                isDestroyed = true;

                //Stop enemy movement
                if (rb != null)
                    rb.velocity = Vector2.zero;

                //Play explosion animation
                animator.SetTrigger("Explode");

                //Play death sound
                if (audioSource != null && deathSound != null)
                    audioSource.PlayOneShot(deathSound);

                //Destroy the bullet that hit
                Destroy(other.gameObject);

                //Increment kill count in game manager
                GameManager.Instance.AddKill();

                //Destroy enemy after delay to allow animation to play
                Destroy(gameObject, destroyDelay);
            }
        }
    }
}
