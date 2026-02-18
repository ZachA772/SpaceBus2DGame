using UnityEngine;
public class BlackHole : MonoBehaviour
{
    [Header("References")]

    //Transform that represents the outer range where the pull effect begins
    [SerializeField] private Transform pullRange;

    //Transform that represents the center of the black hole
    [SerializeField] private Transform deathCircle;

    [Header("Pull Settings")]

    //Minimum pull strength when the player is at the edge of the pull range
    [SerializeField] private float minPullStrength = 0.5f;

    //Maximum pull strength when the player is very close to the center
    [SerializeField] private float maxPullStrength = 6f;

    //Reference to the player Transform
    private Transform player;

    //Flag to track whether the player is currently inside the pull range
    private bool playerInRange = false;

    //Find and stores a reference to the player using the Player tag.
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    //If the player is in range, it calculates and applies a pulling force.
    private void Update()
    {
        //Stop if player is not in range or player reference is missing
        if (!playerInRange || player == null) return;

        //Calculate the current distance between the player and the black hole center
        float distance = Vector2.Distance(
            player.position,
            deathCircle.position
        );

        //Calculate the maximum possible distance within the pull range
        float maxDistance = Vector2.Distance(
            pullRange.position,
            deathCircle.position
        );

        //Convert distance into a normalized value between 0 and 1.
        //Closer distance = higher normalized value
        float normalized = 1f - Mathf.Clamp01(distance / maxDistance);

        //Use a quadratic curve to increase pull strength smoothly.
        //This makes the pull feel stronger closer to the center.
        float pullStrength = Mathf.Lerp(minPullStrength, maxPullStrength, normalized * normalized);

        //Calculate the direction from the player to the black hole center
        Vector2 direction = (deathCircle.position - player.position).normalized;

        //Move the player toward the black hole center
        //Time.deltaTime ensures smooth movement independent of framerate
        player.position += (Vector3)(direction * pullStrength * Time.deltaTime);
    }

    //Called when the player enters the black hole's trigger range
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the object entering is the player
        if (other.CompareTag("Player"))
        {
            //Enable pulling effect
            playerInRange = true;
        }
    }

    //Called when the player exits the black hole's trigger range
    private void OnTriggerExit2D(Collider2D other)
    {
        //Check if the object exiting is the player
        if (other.CompareTag("Player"))
        {
            //Disable pulling effect
            playerInRange = false;
        }
    }

   //Destroy object when it leaves the screen
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
