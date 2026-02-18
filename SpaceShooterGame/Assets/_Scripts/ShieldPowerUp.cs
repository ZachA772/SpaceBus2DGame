using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    [SerializeField] private float duration = 10f;
    //Audio settings for the power up pickup sound
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource; //AudioSource used to play sounds
    [SerializeField] private AudioClip powerUpSound;    //Sound played when powerup is picked up

    //If powerup collides with player, tell player to activate Shield Powerup
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
                player.ActivateShield(duration);

            AudioSource playerAudio = other.GetComponent<AudioSource>();

            //Play the pickup effect
            if (playerAudio != null && powerUpSound != null)
                playerAudio.PlayOneShot(powerUpSound);

            Destroy(gameObject);
        }
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
