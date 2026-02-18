using UnityEngine;


public class Asteroid : MonoBehaviour
{
    //Reference to the Animator component for playing animations
    private Animator animator;

    //Reference to the Rigidbody2D component for physics control
    private Rigidbody2D rb;

    //Flag to prevent the asteroid from being destroyed multiple times
    private bool isDestroyed = false;

    //Reference to the Collider2D component to enable/disable collision
    private Collider2D col;

    //Delay before destroying the asteroid GameObject.
    //Allows time for the explosion animation and sound to finish.
    [SerializeField] private float destroyDelay = 0.6f;

    //Audio settings for the asteroid destruction sound
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource; //AudioSource used to play sounds
    [SerializeField] private AudioClip deathSound;    //Sound played when asteroid is destroyed

    //Retrieves and stores references to required components.
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //If the asteroid has already been destroyed, do nothing.
        if (isDestroyed) return;

        //Check if the object that collided is a player bullet
        if (other.CompareTag("Bullet"))
        {
            //Mark asteroid as destroyed to prevent duplicate triggers
            isDestroyed = true;

            //Disable collision immediately so it cannot harm the player during its death animation
            if (col != null)
                col.enabled = false;

            //Stop asteroid movement
            if (rb != null)
                rb.velocity = Vector2.zero;

            //Trigger the explosion animation
            animator.SetTrigger("Explode");

            //Play the death sound effect
            if (audioSource != null && deathSound != null)
                audioSource.PlayOneShot(deathSound);

            //Destroy the bullet that hit the asteroid
            Destroy(other.gameObject);

            //Notify the GameManager that an enemy has been destroyed,
            //Updates the score and kill counter
            GameManager.Instance.AddKill();

            //Destroy the asteroid after a short delay to allow animation and sound to complete
            Destroy(gameObject, destroyDelay);
        }
    }

    //Destroy object when it leaves the screen
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
