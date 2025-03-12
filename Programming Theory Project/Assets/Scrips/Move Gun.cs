using UnityEngine;

public class MoveGun : MonoBehaviour
{
    // Update is called once per frame
    public float speed = 50.0f;
    private float limit = 57.0f;
    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        // siempre que quieras que un objeto desaparesca deben colocar en signo igual o mayor
        // Destruye el objeto si supera los límites en x o z
        if (transform.position.x >= limit || transform.position.x <= -limit ||
            transform.position.z >= limit || transform.position.z <= -limit)
        {
            Destroy(gameObject);

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colisión detectada con: " + collision.gameObject.name);
        // Verifica si colisiona con un objeto con la etiqueta "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject); // Destruye la bala
        }
    }
}