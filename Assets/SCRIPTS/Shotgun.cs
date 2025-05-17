using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public class Shotgun : Weapon
    {
        public override void Shoot()
        {
            Debug.Log("SPASH");
        }

        public override void Reload()
        {

        }

    }
}