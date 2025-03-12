using UnityEngine;

public class CameraFollow : MonoBehaviour

{

    public Transform target; // Asigna el jugador u otro objetivo en el Inspector
    public Vector3 offset; // Desplazamiento entre la c�mara y el objetivo
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target != null) // Verifica si el objetivo a�n existe
        {
            // Calcula la posici�n deseada sin modificar el eje Y
            Vector3 desiredPosition = new Vector3(
                target.position.x + offset.x, // Suma el desplazamiento en el eje X
                transform.position.y,        // Mantiene la posici�n actual de la c�mara en el eje Y
                target.position.z + offset.z // Suma el desplazamiento en el eje Z
            );

            // Interpolaci�n para un movimiento suave
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition; // Establece la posici�n suavizada
        }
    }
}