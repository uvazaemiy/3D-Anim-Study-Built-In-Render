using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    [SerializeField] private float volume = 1;

    private AudioSource audioSource;

    
    
    
    
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            audioSource.Play();
        }
    }
}
