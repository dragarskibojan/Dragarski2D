using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float resetTime = 5f;

    private float lifetime;
    private float direction;
    private bool hit;
    private BoxCollider2D coll;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        hit = false;

        gameObject.SetActive(true);
        if (coll != null) coll.enabled = true;

        // Вртење на стрелата/огнот лево-десно
        Vector3 localScale = transform.localScale;
        if (Mathf.Sign(localScale.x) != _direction)
        {
            localScale.x = -localScale.x;
        }
        transform.localScale = localScale;
    }

    private void Update()
    {
        if (hit) return;

        // Движење
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        // Времетраење
        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
        {
            gameObject.SetActive(false);
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверка и нанесување штета директно
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<Health>(out Health playerHealth))
            {
                playerHealth.TakeDamage(damage);
            }
        }

        hit = true;
        if (coll != null) coll.enabled = false;
        gameObject.SetActive(false);
    }
}