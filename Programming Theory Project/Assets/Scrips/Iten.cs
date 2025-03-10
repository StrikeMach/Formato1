using UnityEngine;

public class Iten : MonoBehaviour
{
    private float positionFloat = -1.0f;
    private float positionFloat2 = -0.4f;
    private float speed = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Calcula el nuevo valor de la posición usando Mathf.PingPong
        float range = Mathf.Abs(positionFloat - positionFloat2);
        float newPosition = Mathf.PingPong(Time.time * speed, range) + Mathf.Min(positionFloat, positionFloat2);

        // Aplica el movimiento al objeto
        transform.position = new Vector3( transform.position.x, newPosition, transform.position.z);
    }
}
