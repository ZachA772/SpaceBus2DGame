using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource audioSource;

    private void Awake()
    {
        // Singleton logic
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Get AudioSource immediately
            audioSource = GetComponent<AudioSource>();

            // Start music immediately
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
