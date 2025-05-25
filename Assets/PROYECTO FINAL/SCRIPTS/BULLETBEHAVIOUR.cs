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

    [Header("Bullet Hole Decal")]
    public GameObject bulletHoleDecal;
    [Tooltip("The lifetime of the decal in seconds")]
    public float decalLifetime = 10f;
    public float decalOffset = 0.01f;

    private void OnCollisionEnter(Collision collision)
    {
        // Primero manejamos el da�o como antes
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (destroyEnemyOnHit)
            {
                Destroy(collision.gameObject);
            }
            Destroy(gameObject);
            return;
        }

        // L�gica para superficies
        if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            // La "Mexicanada": Cambiamos la bala por el decal
            ContactPoint contact = collision.contacts[0];

            // Instanciamos el decal en la posici�n de la bala
            GameObject decal = Instantiate(bulletHoleDecal,
                                         transform.position,
                                         Quaternion.LookRotation(contact.normal));

            // Ajustamos posici�n exacta
            decal.transform.position = contact.point + contact.normal * decalOffset;

            // Lo hacemos hijo de la superficie
            decal.transform.parent = collision.transform;

            // Destruimos el decal despu�s del tiempo
            Destroy(decal, decalLifetime);

            // Destruimos la bala inmediatamente
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, destroyDelay);
        }
    }
}