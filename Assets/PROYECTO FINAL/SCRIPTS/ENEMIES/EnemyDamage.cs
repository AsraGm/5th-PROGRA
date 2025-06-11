using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 1;
    public GameObject deathParticlesPrefab; 
    public GameObject hitParticlesPrefab; 
    private Spawn Spawn;
    private bool isDead = false;

    private void Start()
    {
        Spawn = FindObjectOfType<Spawn>(); // Busca el SpawnManager al inicio
        if (Spawn == null)
        {
            Debug.LogError("No se encontró el SpawnManager!");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("Player"))
        {
            if (hitParticlesPrefab != null)
            {
                Instantiate(hitParticlesPrefab, transform.position, Quaternion.identity);
            }

            Health health = other.GetComponent<Health>();
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

        if (Spawn != null)
        {
            Spawn.OnCharacterKilled(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }
}