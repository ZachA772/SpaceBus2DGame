using UnityEngine;
using System.Collections;

public class Boss3Controller : MonoBehaviour
{
    [Header("Shield Hand Settings")]
    [SerializeField] private GameObject shieldHandPrefab; //Prefab for the shield hand object
    [SerializeField] private Transform shieldSpawnPoint; //Position where the shield hand will spawn
    [SerializeField] private float handMoveSpeed = 5f; //Speed at which the shield hand moves
    [SerializeField] private float maxLeftX = -5f; //Leftmost position the shield hand can reach
    [SerializeField] private float waitTime = 5f; //Time to wait before and after moving

    private GameObject shieldHandInstance; //Reference to the spawned shield hand
    private Vector3 originalPosition; //Stores the original spawn position of the shield hand

    private void Start()
    {
        //Spawn the shield hand when the boss is created
        SpawnShieldHand();
    }

    private void SpawnShieldHand()
    {
        //Do nothing if prefab or spawn point is missing
        if (shieldHandPrefab == null || shieldSpawnPoint == null) return;

        //Instantiate the shield hand at the spawn point
        shieldHandInstance = Instantiate(shieldHandPrefab, shieldSpawnPoint.position, Quaternion.identity);

        //Store the original position so it can return later
        originalPosition = shieldHandInstance.transform.position;

        //Start the coroutine that controls shield hand movement
        StartCoroutine(HandleShieldHandMovement());
    }

    private IEnumerator HandleShieldHandMovement()
    {
        //Stop coroutine if the shield hand does not exist
        if (shieldHandInstance == null) yield break;

        //Loop forever while the boss is alive
        while (true)
        {
            //Wait before moving the hand to the left
            yield return new WaitForSeconds(waitTime);

            //Move the shield hand left until it reaches maxLeftX
            while (shieldHandInstance.transform.position.x > maxLeftX)
            {
                Vector3 pos = shieldHandInstance.transform.position;
                pos.x -= handMoveSpeed * Time.deltaTime;

                //Clamp position so it does not go past maxLeftX
                if (pos.x < maxLeftX) pos.x = maxLeftX;

                shieldHandInstance.transform.position = pos;

                yield return null; //Wait until next frame
            }

            //Wait while the shield hand is in the blocking position
            yield return new WaitForSeconds(waitTime);

            //Move the shield hand back to its original position
            while (shieldHandInstance.transform.position.x < originalPosition.x)
            {
                Vector3 pos = shieldHandInstance.transform.position;
                pos.x += handMoveSpeed * Time.deltaTime;

                //Clamp position so it does not go past original position
                if (pos.x > originalPosition.x) pos.x = originalPosition.x;

                shieldHandInstance.transform.position = pos;

                yield return null; //Wait until next frame
            }
        }
    }

    private void OnDestroy()
    {
        //Destroy the shield hand when the boss is destroyed
        if (shieldHandInstance != null)
            Destroy(shieldHandInstance);
    }
}
