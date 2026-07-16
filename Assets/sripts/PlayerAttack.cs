using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCoolDown;
    [SerializeField] private Transform firePoint; 
    [SerializeField] private GameObject[] fireballs;

    private Animator anim;
    private PlayerMovement playerMovement; 
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>(); 
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && cooldownTimer > attackCoolDown && playerMovement.canAttack())
        {
            Attack();
        }

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        // Го наоѓаме ПРВИОТ СЛОБОДЕН ИНДЕКС на топка што е комплетно исклучена
        int id = FindFireball();

        // Сигурносна проверка: Ако сите 9 топки се веќе во воздух, НЕ пукај за да не ја закочиме некоја од активните
        if (id == -1) return; 

        if (anim != null)
        {
            anim.SetTrigger("attack");
        }
        
        cooldownTimer = 0;

        if (fireballs != null && fireballs.Length > id && fireballs[id] != null)
        {
            // Прво ја позиционираме кај firePoint
            fireballs[id].transform.position = firePoint.position;
            
            // Земаме пристап до Projectile скриптата на ТОЈ точен објект
            Projectile proj = fireballs[id].GetComponent<Projectile>();
            if (proj != null)
            {
                // Ја активираме и ѝ даваме насока (SetDirection во себе веќе прави gameObject.SetActive(true))
                proj.SetDirection(Mathf.Sign(transform.localScale.x));
            }
        }
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            // Враќаме топка само ако е комплетно НЕАКТИВНА во играта
            if (fireballs[i] != null && !fireballs[i].activeInHierarchy)
            {
                return i;
            }
        }
        // Ако сите топки се во воздух во моментов, враќаме -1 (сигнал за чекање)
        return -1; 
    }
}