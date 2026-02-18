using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Continuous Shake")]
    //Controls how strong the shake effect is
    [SerializeField] private float continuousMagnitude = 0.05f;
    //Controls how fast the shake moves
    [SerializeField] private float continuousSpeed = 5f;

    //Stores the camera's original position so shake is relative to it
    private Vector3 originalPosition;

    private void Start()
    {
        //Save the camera's starting local position
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        //Perlin Noise creates smooth random numbers
        //Generate smooth random movement using Perlin Noise for the X axis
        float x = Mathf.PerlinNoise(Time.time * continuousSpeed, 0f) - 0.5f;

        //Generate smooth random movement using Perlin Noise for the Y axis
        float y = Mathf.PerlinNoise(0f, Time.time * continuousSpeed) - 0.5f;

        //Combine X and Y into an offset vector and scale by magnitude
        Vector3 offset = new Vector3(x, y, 0f) * continuousMagnitude;

        //Apply the shake offset relative to original position
        transform.localPosition = originalPosition + offset;

    }
}
