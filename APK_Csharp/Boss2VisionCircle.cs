using UnityEngine;

public class Boss2VisionCircle : MonoBehaviour
{
    //Reference to the target this object will follow (boss pupil)
    private Transform target;

    private void LateUpdate()
    {
        //Do nothing if no target is assigned
        if (target == null) return;

        //Make the vision circle follow the target position
        transform.position = target.position;
    }

    public void SetTarget(Transform newTarget)
    {
        //Assign the target for this object to follow
        target = newTarget;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the player enters the vision circle
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            //Reverse player movement while inside the vision circle
            if (player != null)
                player.SetMovementReversed(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Check if the player exits the vision circle
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            //Restore normal player movement when leaving the vision circle
            if (player != null)
                player.SetMovementReversed(false);
        }
    }
}
