using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    // МОРА ДА ПИШУВА protected A НЕ private!
    [SerializeField] protected float damage = 1f;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<Health>(out Health playerHealth))
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}