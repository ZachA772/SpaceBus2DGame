using UnityEngine;

public class HomingBullets : MonoBehaviour
{
    private Transform target;//The enemy this bullet will home in on
    private float speed;//Speed of the bullet
    private Rigidbody2D rb;//Reference to Rigidbody2D for movement

    public void SetSpeed(float value)
    {
        speed = value;//Set bullet speed externally
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();//Get Rigidbody2D component
        FindClosestEnemy();//Find the nearest enemy to home in on
    }

    private void FixedUpdate()
    {
        if (target == null)//If no target exists, move straight
        {
            rb.velocity = Vector2.right * speed;
            return;
        }

        //Calculate direction toward target
        Vector2 direction = ((Vector2)target.position - rb.position).normalized;
        rb.velocity = direction * speed;//Move bullet toward target
    }

    private void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");

        float closestDistance = Mathf.Infinity;
        GameObject closest = null;

        //Check normal enemies
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = enemy;
            }
        }

        //Check bosses
        foreach (GameObject boss in bosses)
        {
            float distance = Vector2.Distance(transform.position, boss.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = boss;
            }
        }

        if (closest != null)
            target = closest.transform;
    }

}
