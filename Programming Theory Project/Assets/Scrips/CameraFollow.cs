using UnityEngine;

public class CameraFollow : MonoBehaviour

{

    public Transform target; // Asigna el jugador u otro objetivo en el Inspector
    public Vector3 offset; // Desplazamiento entre la cámara y el objetivo
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target != null) // Verifica si el objetivo aún existe
        {
            // Calcula la posición deseada sin modificar el eje Y
            Vector3 desiredPosition = new Vector3(
                target.position.x + offset.x, // Suma el desplazamiento en el eje X
                transform.position.y,        // Mantiene la posición actual de la cámara en el eje Y
                target.position.z + offset.z // Suma el desplazamiento en el eje Z
            );

            // Interpolación para un movimiento suave
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition; // Establece la posición suavizada
        }
    }
}