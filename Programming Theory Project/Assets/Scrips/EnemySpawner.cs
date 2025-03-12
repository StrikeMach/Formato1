using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Prefabs de los enemigos
    public Transform[] spawnPositions; // Posiciones fijas donde los enemigos aparecerán
    public int currentWave = 1; // Ola inicial

    private int enemiesRemaining; // Enemigos activos en la ola actual

    void Start() => SpawnWave(); // Generar la primera ola

    void Update()
    {
        // Generar nueva ola si no quedan enemigos
        if (enemiesRemaining <= 0)
        {
            currentWave++;
            SpawnWave();
        }
    }

    void SpawnWave()
    {
        Debug.Log("Generando nueva ola.");
        enemiesRemaining = currentWave;
        for (int i = 0; i < currentWave; i++)
        {
            // Crear una posición de spawn
            var prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Vector3 spawnPosition = spawnPositions.Length > 0
                ? spawnPositions[i % spawnPositions.Length].position
                : new Vector3(Random.Range(-50f, 50f), prefab.transform.position.y, Random.Range(-50f, 50f));

            // Instanciar enemigo y suscribir evento
            var spawnedEnemy = Instantiate(prefab, spawnPosition, Quaternion.identity);
            var enemyScript = spawnedEnemy.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                enemyScript.OnDestroyed += () =>
                {
                    enemiesRemaining--; // Reducir la cuenta de enemigos restantes
                    Debug.Log($"Enemy destroyed. Remaining enemies: {enemiesRemaining}");
                };
            }
            else
            {
                Debug.LogError("El prefab no tiene el script Enemy asignado.");
            }
        }
    }

}
