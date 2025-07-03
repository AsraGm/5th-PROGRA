using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Player.Weapon;

namespace Player
{
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField] private Weapon[] weapons;
        [SerializeField] private Transform weaponHolder;
        [SerializeField] private float detectionRange = 5f;
        [SerializeField] private LayerMask weaponLayer;
        [SerializeField] private TextMeshProUGUI ammoText;
        [SerializeField] private Transform pointray;

        private List<Weapon> weaponList = new List<Weapon>();
        private int currentWeaponIndex = 0;
        private Weapon actualWeapon;
        private Action Shoot;

        RaycastHit hit;

        private void Start()
        {
            Debug.Log("[WeaponHandler] Inicializando sistema de armas");

            if (weapons != null && weapons.Length < 1)
            {
                Debug.LogWarning("[WeaponHandler] No hay armas asignadas al WeaponHandler.");
            }
            else
            {
                Debug.Log($"[WeaponHandler] Sistema listo con {weapons.Length} armas iniciales");
            }
        }

        private void Update()
        {
            HandleWeaponSwitch();
            HandleWeaponPickup();

            // Solo dispara si tenemos un arma equipada
            if (actualWeapon != null)
            {
                Shoot?.Invoke();
                actualWeapon?.Reload();
            }
        }

        private void HandleWeaponSwitch()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                int previousIndex = currentWeaponIndex;
                if (scroll > 0)
                {
                    currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;
                    Debug.Log($"[WeaponHandler] Cambiando a siguiente arma. Nuevo índice: {currentWeaponIndex}");
                }
                else
                {
                    currentWeaponIndex = (currentWeaponIndex - 1 + weapons.Length) % weapons.Length;
                    Debug.Log($"[WeaponHandler] Cambiando a arma anterior. Nuevo índice: {currentWeaponIndex}");
                }

                if (previousIndex != currentWeaponIndex)
                {
                    Debug.Log($"[WeaponHandler] Equipando arma en índice {currentWeaponIndex}");
                    EquipWeapon(currentWeaponIndex);
                }
            }
        }

        private void EquipWeapon(int index)
        {
            // Validar índice
            if (index < 0 || index >= weapons.Length)
            {
                Debug.LogError($"[WeaponHandler] Índice de arma inválido: {index}");
                return;
            }

            // Desactivar todas las armas primero
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].gameObject.SetActive(i == index);
                if (i == index)
                {
                    Debug.Log($"[WeaponHandler] Activando arma {weapons[i].name}");
                }
            }

            actualWeapon = weapons[index];
            Debug.Log($"[WeaponHandler] Arma actual: {actualWeapon.name}");

            // Configurar el tipo de disparo
            switch (actualWeapon.fireType)
            {
                case FireType.Automatic:
                    Shoot = AutomaticShoot;
                    Debug.Log("[WeaponHandler] Modo de disparo: Automático");
                    break;
                case FireType.SemiAutomatic:
                    Shoot = SemiAutomaticShoot;
                    Debug.Log("[WeaponHandler] Modo de disparo: Semi-Automático");
                    break;
                default:
                    Debug.LogWarning("[WeaponHandler] Tipo de disparo no reconocido");
                    break;
            }

            // Actualizar UI de munición
            if (ammoText != null)
            {
                ammoText.text = $"{actualWeapon.currentAmmo}/{actualWeapon.ammo}";
                Debug.Log($"[WeaponHandler] Munición actualizada: {ammoText.text}");
            }
        }

        private void AutomaticShoot()
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Debug.Log("[WeaponHandler] Disparo automático detectado");
                actualWeapon.Shoot();
            }
        }

        private void SemiAutomaticShoot()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("[WeaponHandler] Disparo semi-automático detectado");
                actualWeapon.Shoot();
            }
        }

        private void HandleWeaponPickup()
        {
            if (DetectionWeapon())
            {
                Debug.Log("[WeaponHandler] Arma detectada en rango");

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("[WeaponHandler] Tecla E presionada - Intentando recoger arma");
                    Weapon pickedWeapon = hit.collider.GetComponent<Weapon>();

                    if (pickedWeapon != null)
                    {
                        if (!weaponList.Contains(pickedWeapon))
                        {
                            Debug.Log($"[WeaponHandler] Recogiendo arma: {pickedWeapon.name}");

                            weaponList.Add(pickedWeapon);
                            weapons = weaponList.ToArray();
                            Debug.Log($"[WeaponHandler] Total de armas recogidas: {weapons.Length}");

                            pickedWeapon.transform.SetParent(weaponHolder);
                            pickedWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                            pickedWeapon.GetComponent<Collider>().enabled = false;
                            Debug.Log("[WeaponHandler] Arma emparentada y colisionador desactivado");

                            currentWeaponIndex = weapons.Length - 1;
                            Debug.Log($"[WeaponHandler] Nuevo índice de arma actual: {currentWeaponIndex}");
                            EquipWeapon(currentWeaponIndex);
                        }
                        else
                        {
                            Debug.Log("[WeaponHandler] Esta arma ya está en la colección");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("[WeaponHandler] Objeto detectado no tiene componente Weapon");
                    }
                }
            }
        }

        private bool DetectionWeapon()
        {
            bool weaponDetected = Physics.Raycast(pointray.position, transform.forward, out hit, detectionRange, weaponLayer);
            if (weaponDetected)
            {
                Debug.DrawRay(pointray.position, transform.forward * detectionRange, Color.green);
            }
            else
            {
                Debug.DrawRay(pointray.position, transform.forward * detectionRange, Color.red);
            }
            return weaponDetected;
        }

        private void OnDrawGizmos()
        {
            if (pointray == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(pointray.position, transform.forward * detectionRange);
        }
    }
}