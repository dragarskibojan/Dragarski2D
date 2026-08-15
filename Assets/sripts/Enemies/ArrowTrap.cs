using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCoolDown = 2f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
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

        int fireballIndex = FindFireball();

        if (fireballIndex != -1)
        {
            fireballs[fireballIndex].transform.position = firePoint.position;
            fireballs[fireballIndex].SetActive(true);
            EnemyProjectile projectile = fireballs[fireballIndex].GetComponent<EnemyProjectile>();
            if (projectile != null)
            {
                projectile.SetDirection(Mathf.Sign(transform.localScale.x));
            }
        }
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (fireballs[i] != null && !fireballs[i].activeInHierarchy)
            {
                return i;
            }
        }
        return -1; 
    }
}