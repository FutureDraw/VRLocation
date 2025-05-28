using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioSource audioSource;
    public float movementThreshold = 0.01f;

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;

        if (audioSource == null)
        {
            Debug.LogError("AudioSource не назначен!");
        }
    }

    void Update()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        if (distanceMoved > movementThreshold)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Pause();
        }

        lastPosition = transform.position;
    }
}
