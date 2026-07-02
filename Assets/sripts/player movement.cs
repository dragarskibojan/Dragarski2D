using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 7f;

    private Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Движење
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);
        // Vrtenje na igracot levo-desno
    if (horizontalInput > 0.01f)
    {
        transform.localScale = new Vector3(1, 1, 1);
    }
    else if (horizontalInput < -0.01f)
    {
        transform.localScale = new Vector3(-1, 1, 1);
    }
      

        // Скок
        if (Input.GetKeyDown(KeyCode.Space))
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        }
    }
}