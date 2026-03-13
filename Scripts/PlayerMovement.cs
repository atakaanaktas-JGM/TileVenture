using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;
    [SerializeField] float jumpSpeed = 3f;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] Vector2 deathVector = new Vector2(0f, 25f);

    [Header("SFX")]
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip throwSound;
    [SerializeField] AudioClip climbSound;
    [SerializeField] float climbStepInterval = 0.28f;
    [SerializeField] float climbVolume = 0.6f;

    AudioSource sfxSource;

    [Header("Combat")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gunPos;

    CapsuleCollider2D bodyCollider;
    BoxCollider2D playerCollider;
    Rigidbody2D rb;
    Animator animator;

    Vector2 moveInput;
    float gravityAtStart;
    float climbTimer = 0f;
    bool isAlive = true;

    const string ISRUNNING_STRING = "isRunning";
    const string GROUNDLAYER_STRING = "Ground";

    void Start()
    {
        sfxSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        gravityAtStart = rb.gravityScale;
    }

    void Update()
    {
        if (!isAlive) return;

        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) return;
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) return;
        if (!playerCollider.IsTouchingLayers(LayerMask.GetMask(GROUNDLAYER_STRING))) return;

        if (value.isPressed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
            sfxSource.PlayOneShot(jumpSound, 0.5f);
        }
    }

    void OnAttack(InputValue value)
    {
        if (!isAlive) return;

        if (value.isPressed)
        {
            Instantiate(bullet, gunPos.position, Quaternion.identity);
            sfxSource.PlayOneShot(throwSound, 0.5f);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
        rb.linearVelocity = playerVelocity;

        bool hasSpeed = Mathf.Abs(rb.linearVelocity.x) > Mathf.Epsilon;
        animator.SetBool(ISRUNNING_STRING, hasSpeed);
    }

    void FlipSprite()
    {
        if (Mathf.Abs(rb.linearVelocity.x) > Mathf.Epsilon)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.linearVelocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Climb")))
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, moveInput.y * climbSpeed);

            bool isClimbing = Mathf.Abs(moveInput.y) > Mathf.Epsilon;
            animator.SetBool("isClimbing", isClimbing);

            if (isClimbing && climbSound != null)
            {
                climbTimer += Time.deltaTime;

                if (climbTimer >= climbStepInterval)
                {
                    sfxSource.pitch = Random.Range(0.95f, 1.05f);
                    sfxSource.PlayOneShot(climbSound, climbVolume);
                    climbTimer = 0f;
                }
            }
            else
            {
                climbTimer = 0f;
            }
        }
        else
        {
            rb.gravityScale = gravityAtStart;
            animator.SetBool("isClimbing", false);
            climbTimer = 0f;
        }
    }

    void Die()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;

            sfxSource.PlayOneShot(deathSound, 2f);
            animator.SetBool(ISRUNNING_STRING, false);
            animator.SetTrigger("Dying");

            rb.linearVelocity = deathVector;
            FindAnyObjectByType<GameSession>().ProcessPlayerDeath();
        }
    }
}
