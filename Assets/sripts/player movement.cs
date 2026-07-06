using UnityEngine;

public class AdvancedPlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private float climbSpeed = 5f;

    [Header("Surfaces & Collisions")]
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
        // 1. Собирање на влезови (Inputs)
        horizontalInput = Input.GetAxisRaw("Horizontal"); // GetAxisRaw дава подиректна и поостра контрола
        verticalInput = Input.GetAxisRaw("Vertical");

        // 2. Справување со визуелно свртување (Flip)
        HandleSpriteFlip();

        // 3. Главна физичка логика (Движење, Скокање, Качување)
        HandleMovement();

        // 4. Ажурирање на анимации во Animator
        UpdateAnimator();
    }

    private void HandleMovement()
    {
        // Проверка дали играчот се наоѓа на ѕид и е во воздух
        if (IsTouchingWall() && !IsGrounded())
        {
            // Качување по ѕид (Исклучи гравитација и движи се вертикално)
            body.gravityScale = 3f;
            body.linearVelocity = new Vector2(horizontalInput * (speed * 0.3f), verticalInput * climbSpeed);
        }
        else
        {
            // Нормално движење на земја/воздух (Врати гравитација)
            body.gravityScale = 1f;
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

            // Скокање од земја
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                PlayerJump();
            }
        }
    }

    private void PlayerJump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        anim.SetTrigger("jump");
    }

    private void HandleSpriteFlip()
    {
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void UpdateAnimator()
    {
        anim.SetBool("run", Mathf.Abs(horizontalInput) > 0.01f);
        anim.SetBool("grounded", IsGrounded());
        // Ако имаш анимација за качување, одкоментирај ја линијата подолу:
        // anim.SetBool("climbing", IsTouchingWall() && !IsGrounded() && verticalInput != 0);
    }

    // --- СИСТЕМ ЗА ДЕТЕКЦИЈА НА ПОДЛОГИ (Идентичен концепт како во видеото) ---

    private bool IsGrounded()
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

    private bool IsTouchingWall()
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
}