using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float resetTime = 3f; // По колку секунди сама да се изгаси ако не удри ништо

    private float direction;
    private bool hit;
    private float lifetime; // Тајмер кој брои секунди

    private BoxCollider2D boxCollider;
    private Animator anim;
    
    private float timeSinceSpawning;
    private float safeTimeBeforePlayerCollision = 0.15f; 

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        // Секој фрејм мериме колку време топката лета во воздухот
        timeSinceSpawning += Time.deltaTime;

        if (hit) return;

        // Движење напред
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        // АВТОМАТСКО ГАСЕЊЕ НАДВОР ОД СЦЕНАТА:
        lifetime += Time.deltaTime;
        if (lifetime >= resetTime)
        {
            gameObject.SetActive(false); // Ја гасиме топката за пак да биде слободна во низата
        }
    }

    public void SetDirection(float _direction)
    {
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        timeSinceSpawning = 0f; 
        lifetime = 0f; // ГО РЕСЕТИРАМЕ ТАЈМЕРОТ за секое ново пукање!

        if (boxCollider != null)
        {
            boxCollider.enabled = true;
        }

        // Сврти ја топката лево/десно
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;
        }
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (timeSinceSpawning < safeTimeBeforePlayerCollision)
            {
                return;
            }
        }

        StartCoroutine(ExplodeSequence());
    }

    private IEnumerator ExplodeSequence()
    {
        hit = true;

        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }

        if (anim != null)
        {
            anim.SetTrigger("explode");
        }

        yield return new WaitForSeconds(0.4f);

        gameObject.SetActive(false);
    }
}