using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Healths : MonoBehaviour, IDamageables
{
    [SerializeField] private TMP_Text vidaTxt;
    [SerializeField] private Image[] corazones;

    int maxHealth = 5;
    public int health;

    public Apuntes apuntes;

 

    public void KillChar()
    {
        Spawn spawn = FindObjectOfType<Spawn>();
        spawn.OnCharacterKilled(this.gameObject);
    }
}
