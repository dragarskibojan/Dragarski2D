using UnityEngine;

public class HeallthCollectable : MonoBehaviour
{
    [SerializeField] private float HealthValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().AddHealth(HealthValue);
            gameObject.SetActive(false);
        }
    }
}
