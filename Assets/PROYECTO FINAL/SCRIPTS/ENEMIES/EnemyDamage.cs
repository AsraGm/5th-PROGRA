using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 1;
    public GameObject deathParticlesPrefab; 
    public GameObject hitParticlesPrefab; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hitParticlesPrefab != null)
            {
                Instantiate(hitParticlesPrefab, transform.position, Quaternion.identity);
            }

            Health health = other.GetComponent<Health>() ??
                         other.GetComponentInParent<Health>() ??
                         other.GetComponentInChildren<Health>();

            if (health != null)
            {
                health.GetDamage(damage);
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (deathParticlesPrefab != null)
        {
            Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}