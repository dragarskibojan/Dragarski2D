using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement & Jumping")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Wall Mechanics")]
    [SerializeField] private float climbSpeed = 5f; 
    private float wallJumpCooldown;

    [Header("Collisions & Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private BoxCollider2D boxCollider;

    private Rigidbody2D body;
    private Animator anim;
    private float horizontalInput;
    private float verticalInput;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical"); 

        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        anim.SetBool("run", Mathf.Abs(horizontalInput) > 0.01f);
        anim.SetBool("grounded", isGrounded());

        if (wallJumpCooldown < 0.2f)
        {
            if (isWall() && !isGrounded())
            {
                body.gravityScale = 0; 
                body.linearVelocity = new Vector2(horizontalInput * (speed * 0.3f), verticalInput * climbSpeed);
            }
            else
            {
                body.gravityScale = 1; 
                body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded())
                    Jump();
                else if (isWall() && !isGrounded())
                    WallJump(); 
            }
        }
        else
        {
            wallJumpCooldown -= Time.deltaTime;
        }
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        anim.SetTrigger("jump");
    }

    private void WallJump()
    {
        body.linearVelocity = new Vector2(-transform.localScale.x * speed, jumpForce);
        wallJumpCooldown = 0.5f; 
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0f,
            Vector2.down,
            0.1f,
            groundLayer);

        return raycastHit.collider != null;
    }

    private bool isWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0f,
            new Vector2(transform.localScale.x, 0),
            0.1f,
            wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !isWall();
    }
}