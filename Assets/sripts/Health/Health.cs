using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth = 3f;

    public float currentHealth { get; private set; }

    private Animator anim;
    private bool dead;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        if (dead) return; 

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            // Invincibility frames (iframes)
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
}