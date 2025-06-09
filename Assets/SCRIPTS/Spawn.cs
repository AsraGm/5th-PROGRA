using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject char2Spawn; //personaje a spawnear
    [SerializeField] private Transform[] spawnPoints; 

    [SerializeField] private int maxCharInScene;
    [SerializeField] private int maxCharInstanceInQueue; //max de personje disponibles para usarse

    [SerializeField] private float spawnRate; //puntos de personajes

    [SerializeField] Queue<GameObject> charQueue; // Esta va a ser la fila de los personajes

    [SerializeField] int charinScene = 0;//personajes en escena

    void Start()
    {
        StartPool();
    }

    private void StartPool()
    {
        charQueue = new Queue<GameObject>();
        for (int i = 0; i < maxCharInstanceInQueue; i++)
        {
            GameObject instance = Instantiate(char2Spawn); //Instancia
            instance.SetActive(false); //Desactiva
            charQueue.Enqueue(instance); //Agrega a la fila
        }
        StartCoroutine(SpawnCharacters());
    }

    private IEnumerator SpawnCharacters()
    {
        yield return new WaitUntil(() => charinScene < maxCharInScene);

        for (int i = charinScene; i < maxCharInScene; i++)
        { 
            yield return new WaitForSeconds(spawnRate);
            GameObject character = charQueue.Dequeue();
            character.SetActive(true);
            int randomSpawn = RandomSpawnPoint();
            character.transform.position = spawnPoints[randomSpawn].position;
            character.transform.rotation = spawnPoints[randomSpawn].rotation;
            charinScene++;
        }
        StartCoroutine(SpawnCharacters());
    }

    private int RandomSpawnPoint()
    {
        return Random.Range(0, spawnPoints.Length);
    }

    public void OnCharacterKilled(GameObject killedCharacter)
    { 
        killedCharacter.SetActive(false);
        charQueue.Enqueue(killedCharacter);
        charinScene--;
    }
}
