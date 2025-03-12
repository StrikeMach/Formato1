using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    private float spawnRange = 50f; // Rangos (-60 a 60)
    public float spawnInterval = 10f; // Intervalo entre spawns (10 segundos)
    private GameObject currentItem; // Referencia al objeto actual

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Comienza la rutina de generación de objetos
        StartCoroutine(SpawnItemRoutine());
    }

    IEnumerator SpawnItemRoutine()
    {
        while (true) // Rutina infinita para reaparecer objetos
        {
            // Espera el tiempo de intervalo entre spawns
            yield return new WaitForSeconds(spawnInterval);

            // Verifica si ya hay un objeto activo
            if (currentItem == null)
            {
                // Genera un nuevo objeto en una posición aleatoria
                SpawnItem();
            }
        }
    }
    void SpawnItem()
    {
        // Genera coordenadas aleatorias dentro del rango especificado
        float randomX = Random.Range(-spawnRange, spawnRange);
        float randomZ = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnPosition = new Vector3(randomX, 0, randomZ); // Ajusta el eje Y según tu escena

        // Crea el objeto y almacena la referencia
        currentItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);

        // Inicia el temporizador para destruir el objeto después de 30 segundos
        StartCoroutine(DestroyItemAfterTime(currentItem, 30f));
    }
    IEnumerator DestroyItemAfterTime(GameObject item, float delay)
    {
        // Espera el tiempo especificado
        yield return new WaitForSeconds(delay);

        // Si el objeto aún no ha sido destruido, lo destruye
        if (item != null)
        {
            Destroy(item);
        }
    }

}
