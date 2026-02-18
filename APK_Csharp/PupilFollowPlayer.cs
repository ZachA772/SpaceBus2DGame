using UnityEngine;

public class PupilFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform eyeCenter;   // empty object at center of the eye
    [SerializeField] private float maxOffset = 0.2f; // how far pupil can move
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void LateUpdate()
    {
        if (player == null || eyeCenter == null) return;

        Vector2 dir = (player.position - eyeCenter.position).normalized;

        // Move pupil toward player but keep it inside the eye
        transform.position = eyeCenter.position + (Vector3)(dir * maxOffset);
    }
}