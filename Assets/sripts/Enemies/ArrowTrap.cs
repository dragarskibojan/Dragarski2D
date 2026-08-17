using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCoolDown = 2f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] arrows; 
    private float cooldownTimer;

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCoolDown)
        {
            Attack();
        }
    }

    private void Attack()
    {
        cooldownTimer = 0;

        int arrowIndex = FindArrow();

        if (arrowIndex != -1)
        {
            arrows[arrowIndex].transform.position = firePoint.position;
            EnemyProjectile projectile = arrows[arrowIndex].GetComponent<EnemyProjectile>();
            if (projectile != null)
            {
                projectile.SetDirection(Mathf.Sign(transform.localScale.x));
            }
        }
    }

    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (arrows[i] != null && !arrows[i].activeInHierarchy)
            {
                return i;
            }
        }
        return -1; 
    }
}