using System.Collections;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab de la esfera
    public Transform launchPoint; // Punto de lanzamiento (puedes usar un Empty GameObject)
    public float projectileSpeed = 10f; // Velocidad del proyectil
    public float maxDistance = 15f; // Distancia máxima antes de destruir el proyectil

    private bool canShoot = true; // Controla si el jugador puede disparar

    void Update()
    {
        // Detecta si se presiona la tecla Z
        if (Input.GetKeyDown(KeyCode.Z) && canShoot)
        {
            StartCoroutine(ShootProjectile());
        }
    }

    IEnumerator ShootProjectile()
    {
        // Desactiva la capacidad de disparar por 1 segundo
        canShoot = false;

        // Espera 1 segundo antes de disparar
        yield return new WaitForSeconds(0.5f);

        // Crea el proyectil en la posición del launchPoint
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);

        // Añade fuerza al proyectil para lanzarlo
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse); // Lanza el proyectil hacia adelante
        }

        // Inicia la corrutina para destruir el proyectil si se aleja demasiado
        StartCoroutine(DestroyProjectileIfTooFar(projectile, launchPoint.position));

        // Permite disparar de nuevo después de 1 segundo
        canShoot = true;
    }

    IEnumerator DestroyProjectileIfTooFar(GameObject projectile, Vector3 startPosition)
    {
        while (projectile != null)
        {
            // Calcula la distancia en los ejes X y Z
            float distanceX = Mathf.Abs(projectile.transform.position.x - startPosition.x);
            float distanceZ = Mathf.Abs(projectile.transform.position.z - startPosition.z);

            // Si supera la distancia máxima, destruye el proyectil
            if (distanceX > maxDistance || distanceZ > maxDistance)
            {
                Destroy(projectile);
                yield break;
            }

            // Espera un frame antes de volver a verificar
            yield return null;
        }
    }
}