using UnityEngine;

public class StrafeEnemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;//Speed at which enemy moves toward player
    [SerializeField] private float strafeAmplitude = 1.5f;//How far enemy strafes side-to-side
    [SerializeField] private float strafeFrequency = 3f;//Speed of strafing oscillation

    private Transform player;//Reference to player

    [Header("Death")]
    [SerializeField] private float destroyDelay = 0.3f;//Delay before destroying enemy after death animation

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;//Audio source for sounds
    [SerializeField] private AudioClip deathSound;//Sound played on death

    private Animator animator;//Handles animations
    private Rigidbody2D rb;//Rigidbody for physics
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

        //Direction toward player
        Vector2 toPlayer = (player.position - transform.position).normalized;

        //Perpendicular vector for strafing
        Vector2 perpendicular = new Vector2(-toPlayer.y, toPlayer.x);

        //Calculate side-to-side strafe offset
        float strafe = Mathf.Sin(Time.time * strafeFrequency) * strafeAmplitude;

        //Combine forward movement and strafing
        Vector2 movement =
            toPlayer * moveSpeed +
            perpendicular * strafe;

        //Apply movement
        transform.position += (Vector3)(movement * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDestroyed) return;

        if (other.CompareTag("Bullet"))
        {
            isDestroyed = true;

            //Stop movement
            if (rb != null)
                rb.velocity = Vector2.zero;

            //Trigger death animation
            if (animator != null)
                animator.SetTrigger("OnDeath");

            //Play death sound
            if (audioSource != null && deathSound != null)
                audioSource.PlayOneShot(deathSound);

            //Destroy the bullet that hit
            Destroy(other.gameObject);

            //Increment kill count
            GameManager.Instance.AddKill();

            //Destroy enemy after delay
            Destroy(gameObject, destroyDelay);
        }
    }
}
