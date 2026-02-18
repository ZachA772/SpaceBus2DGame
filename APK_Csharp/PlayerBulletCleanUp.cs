using UnityEngine;

public class PlayerBulletCleanUp : MonoBehaviour
{
    private bool isDestroyed = false;//Tracks if the bullet has already hit something
    private bool isShieldShot = false;//Tracks if the bullet hit a shield or boss feature

    void OnBecameInvisible()
    {
        Destroy(gameObject);//Destroy the bullet when it leaves the camera view
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDestroyed) return;//Ignore if already destroyed

        if (other.CompareTag("EnemyShield") || other.CompareTag("BossFeature"))
        {
            isDestroyed = true;//Mark as destroyed
            isShieldShot = true;//Mark as shield shot

            Destroy(gameObject);//Destroy the bullet on contact
        }
    }

    public bool GetShieldShot()
    {
        return isShieldShot;//Return whether this bullet hit a shield
    }

    public void SetShieldShot()
    {
        isShieldShot = false;//Reset shield shot flag
    }
}
