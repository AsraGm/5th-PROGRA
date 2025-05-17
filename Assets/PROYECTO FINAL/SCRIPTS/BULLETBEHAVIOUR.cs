using UnityEngine;

public class BULLETBEHAVIOUR : MonoBehaviour
{
    [Header("Collision Settings")]
    public LayerMask groundLayer;
    public float destroyDelay = 0f;

    [Header("Enemy Damage")]
    public bool destroyEnemyOnHit = true;
    public int damage = 100;
    public float pushBackForce = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            Destroy(gameObject, destroyDelay);
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (destroyEnemyOnHit)
            {
                Destroy(collision.gameObject); 
            }

            Destroy(gameObject); 
        }
    }
}