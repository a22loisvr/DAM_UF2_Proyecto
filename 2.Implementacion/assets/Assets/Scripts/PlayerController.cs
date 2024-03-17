using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float speed = 3.0f;
    [SerializeField] float jumpForce = 5.0f;
    [SerializeField] Vector2 wallJumpForce;
    [SerializeField] float WallJumpDuration = 0.1f;
    [SerializeField] float wallSlideSpeed;
    Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask hazardLayer;
    [SerializeField] LayerMask goalLayer;
    [SerializeField] Transform wallCheck;
    bool isWallSliding;
    bool isWallJumping;
    private float h;
    private float startDir;
    private float wallJumpDir;

    private void Awake()
    {
        AudioManager audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseGame();
        }

        h = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(h));
        animator.SetBool("isJumping", !isGrounded());
        animator.SetBool("isWallSliding", isWallSliding);
        animator.SetBool("isDead", dead());

        if (dead())
        {
            StartCoroutine(Respawn());
        }
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (reachedGoal())
        {
            NextLevel();
        }

        if (isTouchingWall() && !isGrounded() && h != 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }

    }

    private void FixedUpdate()
    {
        if (!dead() && !reachedGoal())
        {
            Move();
        }

    }

    private void Flip()
    {
        Vector3 Scale = transform.localScale;
        if (rb.velocity.x < -0.1f)
        {
            Scale.x = Mathf.Abs(Scale.x) * -1;
            startDir = -1;

        }
        if (rb.velocity.x > 0.1f)
        {
            Scale.x = Mathf.Abs(Scale.x);
            startDir = 1;
        }
        transform.localScale = Scale;
    }

    private void Move()
    {
        if (Mathf.Abs(rb.velocity.x) > speed)
        {
            rb.velocity = new Vector2(h * speed, rb.velocity.y);
        }
        if (isGrounded() && rb.velocity.x < speed && rb.velocity.x > -speed)
        {
            rb.velocity = new Vector2(h * speed, rb.velocity.y);
        }
        if (!isGrounded())
        {
            airMovement();
        }
        if (isWallJumping)
        {
            rb.velocity = new Vector2(wallJumpForce.x * wallJumpDir, wallJumpForce.y);
        }
        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            Flip();
        }
    }

    private void Jump()
    {
        if (isGrounded())
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.jump);
            rb.velocity = new Vector2(h * speed, jumpForce);
        }
        if (isWallSliding)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.jump);
            wallJumpDir = -startDir;
            isWallJumping = true;
            Invoke("StopWallJump", WallJumpDuration);
        }
    }

    void StopWallJump()
    {
        isWallJumping = false;
    }

    void airMovement()
    {
        if (rb.velocity.y < 1.5f)
        {
            rb.AddForce(new Vector2(h * speed * 4, 0));
        }

    }

    bool isGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(0.1f, 0.2f), 0, groundLayer);
    }

    bool isTouchingWall()
    {
        return Physics2D.OverlapBox(wallCheck.position, new Vector2(0.2f, 0.1f), 0, groundLayer);
    }

    bool dead()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(0.2f, 0.1f), 0, hazardLayer) || Physics2D.OverlapBox(wallCheck.position, new Vector2(0.1f, 0.2f), 0, hazardLayer);
    }

    bool reachedGoal()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(0.2f, 0.1f), 0, goalLayer) || Physics2D.OverlapBox(wallCheck.position, new Vector2(0.1f, 0.2f), 0, goalLayer);
    }

    IEnumerator Respawn()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.death);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void CloseGame()
    {
        Application.Quit();
    }
}
