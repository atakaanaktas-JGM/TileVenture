using UnityEngine;

public class BounceSoundPlay : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] AudioClip bounceSound;
    AudioSource audioSource;
  
    private void OnCollisionEnter2D(Collision2D collision)
    {
        audioSource.PlayOneShot(bounceSound,0.3f);
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
