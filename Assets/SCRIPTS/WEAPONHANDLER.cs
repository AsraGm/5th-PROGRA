using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Este script debe de realizar la siguientes funciones:
    /// 
    /// TAREA: 1.- Cambiar de arma. Por lo que siempre debe de haber como minimo 2 armas.
    /// El cambio de arma se debe de realizar si o si, usando la rueda del mouse
    /// 
    /// EJERUCICIO / TAREA 2.- Segun el arma equipada debe de disparar según su funcion
    /// 
    /// 3.- Segun el arma equipada debe de recargar de la manera correspondiente
    /// 
    /// 4.- Al cambiar de arma, debe de aparecer en pantalla la municion de esa arma
    /// </summary>
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField] private Weapon[] weapons;
        [SerializeField] private Weapon actualWeapon;

        // Una Action es una variable donde puedes guardar metodos
        private Action Shoot;

        private void Start()
        {
            Shoot = AutomaticShoot;

            switch (actualWeapon) // Weapon
            {
                // Lo que el caso entiende, es Weapon, no AutomaticRifle
                case AutomaticRifle: // AutomaticRifle hereda de Weapon, por lo tanto es Weapon
                    {
                        Shoot = AutomaticShoot;
                        break;
                    }

                    // Weapon
                case Handgun:
                    {
                        Shoot = SemiAutomaticShoot;
                        break;
                    }
                    // Weapon
                case Shotgun:
                {
                        Shoot = SemiAutomaticShoot;

                        break;
                }
            }

        }

        private void Update()
        {
            Shoot();
        }

        private void AutomaticShoot()
        {
            if (actualWeapon.CheckAmmo() && Input.GetMouseButton(0))
            {
                actualWeapon.Shoot();
            }
        }

        private void SemiAutomaticShoot()
        {
            if (actualWeapon.CheckAmmo() && Input.GetMouseButtonDown(0))
            {
                actualWeapon.Shoot();
            }
        }
    }
}