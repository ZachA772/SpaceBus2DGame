using UnityEngine;

public class BossBullet : MonoBehaviour
{
    //Destroy bullet if it goes outside the screen
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    //Destroy bullet if it hits the player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}