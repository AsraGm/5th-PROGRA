using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class AutomaticRifle : Weapon
    {

        public override void Shoot()
        {
            Debug.Log("Ratatatata");
        }

        public override void Reload()
        {

        }

    }
}