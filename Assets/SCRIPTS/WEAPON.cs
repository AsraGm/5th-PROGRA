
using UnityEngine;


namespace Player
{
    public abstract class Weapon : MonoBehaviour
    {

        public string weaponName;

        public int damage; // daño del arma
        public float range; // alcance del arma
        public float fireRate; // cadencia del arma
        public float accuracy; // punteria: Que tanto se mueve el arma o dispara hacia donde apuntas

        public int currentAmmo; // municion de mi cargador actual
        public int currentMaxAmmo; // capacidad maxima de el cargador
        public int ammo; // municion disponible en la reserva
        public int maxAmmo; // capacidad maxima de mi reserva

        public abstract void Shoot(); // Insta

        public abstract void Reload(); // Youtube

        public bool CheckAmmo()
        {
            return currentAmmo <= 0 && ammo <= 0;
        }

    }
}

