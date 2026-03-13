using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    Rigidbody2D rb;
    [SerializeField] float bulletSpeed = 5.0f;
    [SerializeField] AudioClip enemyDeathSound;

    PlayerMovement player;
    float xSpeed;

    const string ENEMY_STRING = "Enemy";
   
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindFirstObjectByType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(xSpeed, 0f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if(collision.gameObject.CompareTag(ENEMY_STRING))
        {
            Destroy(collision.gameObject);
            AudioSource.PlayClipAtPoint(enemyDeathSound, transform.position);
           
        }


        Destroy(gameObject);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject,0.5f);
    }
}
