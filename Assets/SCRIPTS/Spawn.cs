using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [Header("Configuración de Rondas")]
    [SerializeField] private int charactersPerRound = 6;
    [SerializeField] private int charactersKilled = 0;

    [Header("Spawning")]
    [SerializeField] private List<GameObject> enemyPrefabs; // Cambiado a lista
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int maxCharacterCountInScene = 3;
    [SerializeField] private int maxCharacterInstancesinQueue = 20;
    [SerializeField] private float spawnRate = 2f;

    private int charactersSpawnedThisRound = 0;
    private int charactersAliveInScene = 0;
    private float spawnTimer = 0f;

    private void Update()
    {
        if (enemyPrefabs.Count == 0) return; // Seguridad si no hay prefabs

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnRate)
        {
            spawnTimer = 0f;
            TrySpawnCharacter();
        }
    }

    private void TrySpawnCharacter()
    {
        if (charactersSpawnedThisRound >= charactersPerRound) return;
        if (charactersAliveInScene >= maxCharacterCountInScene) return;
        if (maxCharacterInstancesinQueue <= 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.SetActive(true); // Forzar activación

        charactersSpawnedThisRound++;
        charactersAliveInScene++;
        maxCharacterInstancesinQueue--;

        EnemyNotifier notifier = enemy.GetComponent<EnemyNotifier>();
        if (notifier == null) notifier = enemy.AddComponent<EnemyNotifier>();
        notifier.SetSpawner(this);
    }

    public void NotifyEnemyKilled()
    {
        charactersKilled++;
        charactersAliveInScene--;

        if (charactersKilled >= charactersPerRound)
        {
            StartNextRound();
        }
    }

    private void StartNextRound()
    {
        charactersPerRound += 2;
        charactersKilled = 0;
        charactersSpawnedThisRound = 0;
        Debug.Log($"Nueva ronda: {charactersPerRound} enemigos");
    }
}