using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private int charPerRound = 6;
    [SerializeField] private List<GameObject> char2Spawn; //personaje a spawnear
    [SerializeField] private Transform[] spawnPoints; 
    [SerializeField] private int charInScene = 4;//personajes en escena
    [SerializeField] private int maxCharInstanceInQueue; //max de personje disponibles para usarse

    [SerializeField] private float spawnRate; //puntos de personajes

    private int charKilled = 0;
    private int maxCharInScene = 0;
    Queue<GameObject> charQueue; // Esta va a ser la fila de los personajes
    private bool isSpawning = false;

    void Start()
    {
        if (char2Spawn == null || char2Spawn.Count == 0)
        {
            Debug.LogError("No hay prefabs de enemigos asignados en char2Spawn!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No hay puntos de spawn asignados!");
            return;
        }

        StartPool();
    }

    private void StartPool()
    {
        charQueue = new Queue<GameObject>();
        for (int i = 0; i < maxCharInstanceInQueue; i++)
        {
            GameObject prefab = char2Spawn[Random.Range(0, char2Spawn.Count)];
            GameObject instance = Instantiate(prefab);
            instance.name = "Enemy #" + i;
            instance.SetActive(false);
            charQueue.Enqueue(instance);
        }
        StartCoroutine(SpawnInitialWave());
    }

    private IEnumerator SpawnInitialWave()
    {
        Debug.Log($"Iniciando oleada para {charPerRound} enemigos");

        int enemiesToSpawn = Mathf.Min(charPerRound, maxCharInScene - charInScene);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnSingleCharacter();
            yield return new WaitForSeconds(spawnRate);
        }
    }

    private void SpawnSingleCharacter()
    {
        Debug.Log($"Intentando spawnear - EnEscena: {charInScene}, Max: {maxCharInScene}, Cola: {charQueue.Count}");

        if (charInScene >= maxCharInScene)
        {
            Debug.Log("Límite en escena alcanzado");
            return;
        }

        if (charQueue.Count == 0)
        {
            Debug.LogWarning("Cola vacía");
            return;
        }

        GameObject character = charQueue.Dequeue();
        if (character == null)
        {
            Debug.LogError("Enemigo nulo en cola");
            return;
        }

        character.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        character.SetActive(true); 
        charInScene++;

        Debug.Log($"Spawn exitoso: {character.name} en {character.transform.position}");
    }

    public void OnCharacterKilled(GameObject killedCharacter)
    {
        if (killedCharacter == null) return;

        killedCharacter.SetActive(false);
        charQueue.Enqueue(killedCharacter);

        charInScene--;
        charKilled++;

        Debug.Log($"Enemigo eliminado. Restantes: {charPerRound - charKilled}/{charPerRound}");

        if (charKilled < charPerRound)
        {
            StartCoroutine(RespawnWithDelay());
        }
        else
        {
            Debug.Log("Ronda completada. Preparando nueva oleada...");
            charKilled = 0;
            charPerRound += 2;
            StartCoroutine(SpawnInitialWave());
        }
    }

    private IEnumerator RespawnWithDelay()
    {
        yield return new WaitForSeconds(spawnRate); 
        SpawnSingleCharacter();
    }
}
