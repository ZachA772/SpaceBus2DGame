using UnityEngine;

public class Boss2Minion : MonoBehaviour
{
    //Control Minion Movement Speed
    [SerializeField] private float moveSpeed = 5f;

    private void Update()
    {
        //Move the minion downward every frame
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
    }

    void OnBecameInvisible()
    {
        //Destroy the minion when it leaves the screen
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Destroy minion if it hits the player
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
