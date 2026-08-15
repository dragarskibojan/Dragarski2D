 using System.Collections;
using UnityEngine;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage = 1f;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay = 0.5f; // Намалено за брз тест
    [SerializeField] private float activeTime = 2f;

    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered;
    private bool active;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        if (anim == null) anim = GetComponentInChildren<Animator>();

        spriteRend = GetComponent<SpriteRenderer>();
        if (spriteRend == null) spriteRend = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("1. ИГРАЧОТ СТАПНА НА СТАПИЦАТА!");

            if (!triggered)
            {
                StartCoroutine(ActivateFiretrap());
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && active)
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    private IEnumerator ActivateFiretrap()
    {
        triggered = true;
        Debug.Log("2. СТАПИЦАТА Е ТРИГЕРУВАНА (Се чека одложувањето)...");

        if (spriteRend != null) spriteRend.color = Color.red; 

        yield return new WaitForSeconds(activationDelay);

        Debug.Log("3. СЕГА ТРЕБА ДА СЕ ПУШТИ АНИМАЦИЈАТА!");

        if (spriteRend != null) spriteRend.color = Color.white; 
        active = true;

        if (anim != null)
        {
            anim.SetBool("activated", true);
            Debug.Log("4. Вредноста 'activated' е поставена на TRUE!");
        }
        else
        {
            Debug.LogError("ГРЕШКА: Аниматорот е NULL (не е пронајден)!");
        }

        yield return new WaitForSeconds(activeTime);

        active = false;
        triggered = false;

        if (anim != null)
        {
            anim.SetBool("activated", false);
            Debug.Log("5. Стапицата се исклучи.");
        }
    }
}