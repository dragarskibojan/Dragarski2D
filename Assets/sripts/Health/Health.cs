using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth = 3f;

    public float currentHealth { get; private set; }

    private Animator anim;
    private bool dead;
    private bool isInvulnerable;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration; 
    [SerializeField] private int numberOfFlashes;   
    private SpriteRenderer spriteRend;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        if (dead || isInvulnerable) return; 

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invulnerability());
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");

                PlayerMovement playerMovement = GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.enabled = false;
                }

                dead = true;
            }
        }
    }

    public void AddHealth(float value)
    {
        if (dead) return; 

        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingHealth);
    }

    private IEnumerator Invulnerability()
    {
        isInvulnerable = true; 

        Physics2D.IgnoreLayerCollision(10, 11, true);

        float flashDelay = iFramesDuration / (numberOfFlashes * 2);

        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f); 
            yield return new WaitForSeconds(flashDelay);
            
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(flashDelay);
        }

        Physics2D.IgnoreLayerCollision(10, 11, false);
        
        isInvulnerable = false; 
    }
}