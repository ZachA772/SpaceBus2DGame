using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 20;//Maximum health of the boss
    [SerializeField] private float destroyDelay = 0.5f;//Delay before destroying boss after death

    public System.Action OnBossDeath;//Event triggered when the boss dies

    [Header("UI")]
    [SerializeField] private string healthBarName = "BossHealthBar";//Name of the health bar object in the scene
    private Slider healthBar;//Reference to the UI Slider component

    [Header("Audio")]
    [SerializeField] private AudioClip deathSound;//Sound played when the boss dies

    private int currentHealth;//Current health value
    private Animator animator;//Reference to Animator component
    private bool isDead = false;//Prevents multiple death triggers

    private void Start()
    {
        //Initialize health
        currentHealth = maxHealth;

        //Get Animator component
        animator = GetComponent<Animator>();

        //Find the health bar slider in the scene by name
        healthBar = GameObject.Find(healthBarName)?.GetComponent<Slider>();

        //Set the slider max value to match boss health
        if (healthBar != null)
            healthBar.maxValue = maxHealth;

        //Update the UI to reflect starting health
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        //Update the health bar value if it exists
        if (healthBar != null)
            healthBar.value = currentHealth;
    }

    //Handles collision with bullets
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Do nothing if boss is already dead
        if (isDead) return;

        //Check if hit by a bullet
        if (other.CompareTag("Bullet"))
        {
            //Destroy the bullet
            Destroy(other.gameObject);

            //Apply 1 damage per bullet
            TakeDamage(1);
        }
    }

    public void TakeDamage(int amount)
    {
        //Reduce current health
        currentHealth -= amount;

        //Update the UI
        UpdateHealthUI();

        //Check if health has reached zero
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        //Prevent multiple death executions
        isDead = true;

        //Trigger death animation if available
        if (animator != null)
            animator.SetTrigger("BossDeath");

        //Play death sound from Player AudioSource
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && deathSound != null)
        {
            AudioSource playerAudio = player.GetComponent<AudioSource>();

            if (playerAudio != null)
                playerAudio.PlayOneShot(deathSound);
        }

        //Notify any subscribed listeners that the boss has died
        OnBossDeath?.Invoke();

        //Destroy the boss object after delay
        Destroy(gameObject, destroyDelay);
    }
}
