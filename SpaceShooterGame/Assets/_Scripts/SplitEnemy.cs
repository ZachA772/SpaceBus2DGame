using UnityEngine;

public class SplitEnemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;//Speed at which enemy moves toward the player

    [Header("Splitting")]
    [SerializeField] private GameObject splitEnemyPrefab;//Prefab to instantiate when splitting
    [SerializeField] private float splitOffset = 1f;//Vertical offset for split clones
    [SerializeField] private float scaleFactor = 0.5f;//Scale reduction factor for split clones

    private Transform player;//Reference to player for movement

    [Header("Death")]
    [SerializeField] private float destroyDelay = 0.3f;//Delay before destroying enemy after death animation

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;//Audio source for playing sounds
    [SerializeField] private AudioClip deathSound;//Sound played when enemy dies

    private Animator animator;//Handles enemy animations
    private Rigidbody2D rb;//Rigidbody for physics/movement
    private bool isDestroyed = false;//Tracks if enemy has been destroyed

    private void Start()
    {
        //Find player and components
        player = GameObject.FindGameObjectWithTag("Player").transform;
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

    public void Split()
    {
        //Don't split if already small
        if (transform.localScale.x * scaleFactor < 0.05f)
        {
            if (animator != null)
                animator.SetTrigger("OnDeath");

            //Play death sound
            if (audioSource != null && deathSound != null)
                audioSource.PlayOneShot(deathSound);

            Destroy(gameObject, destroyDelay);
            return;
        }

        //Calculate positions for split clones
        Vector3 topPos = transform.position + Vector3.up * splitOffset;
        Vector3 bottomPos = transform.position + Vector3.down * splitOffset;

        //Instantiate top clone
        GameObject topClone = Instantiate(splitEnemyPrefab, topPos, Quaternion.identity);
        topClone.transform.localScale = transform.localScale * scaleFactor;

        //Instantiate bottom clone
        GameObject bottomClone = Instantiate(splitEnemyPrefab, bottomPos, Quaternion.identity);
        bottomClone.transform.localScale = transform.localScale * scaleFactor;

        if (animator != null)
            animator.SetTrigger("OnDeath");

        //Play death sound
        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);

        //Increment kill count
        GameManager.Instance.AddKill();

        //Destroy this enemy after delay
        Destroy(gameObject, destroyDelay);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDestroyed) return;

        if (other.CompareTag("Bullet"))
        {
            isDestroyed = true;

            //Destroy the bullet
            Destroy(other.gameObject);

            //Split enemy into two smaller clones
            Split();
        }
    }
}
