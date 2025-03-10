using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float Range = 67;
    public float speed = 5.0f;
    public float speedRotate = 8.0f;
    //Temporisador de velocidad
    private float boostedSpeed = 0f;

    // Configuración de disparo
    public GameObject gunPrefab; // Prefab del proyectil
    public Transform launchPoint; // Punto desde donde se lanza el proyectil
    private bool canShoot = true; //control de disparo
    private float shootDelay = 0.3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Controles de movimiento
        float forwardInput = Input.GetAxis("Horizontal");
        float HorizontalInput = Input.GetAxis("Vertical");
        //Movimiento del personaje
        transform.Rotate(Vector3.forward * forwardInput * Time.deltaTime * speedRotate);
        transform.Translate(Vector3.down * HorizontalInput * Time.deltaTime * (speed + boostedSpeed));

        //Muro donde el objeto choque
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -Range, Range),
            transform.position.y,
            Mathf.Clamp(transform.position.z, -Range, Range));

        // Detectar la tecla Z para disparar
        if (Input.GetKeyDown(KeyCode.Z) && canShoot)
        {
            StartCoroutine(ShootGun());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Iten"))
        {
            Debug.Log("El jugador ha recogido un objeto.");
                    //Incrementa la velocidad del jugador
                    boostedSpeed = 15f;
            // Aumenta la velocidad de disparo
            shootDelay = Mathf.Max(0.1f, shootDelay - 0.1f); // Disminuir retraso, pero no menos de 0.1s


            //Destruye el objeto Iten
            Destroy(other.gameObject);
            // Inicia la corrutina para resetear la velocidad
            StartCoroutine(ResetSpeed());
        }
    }

    //Temporisador
    IEnumerator ResetSpeed()
    {
        Debug.Log("Inicio de la corrutina ResetSpeed");
        yield return new WaitForSeconds(10f);
        boostedSpeed = 0f;
        shootDelay = 0.3f;
        Debug.Log("Fin de la corrutina, boostedSpeed reseteado a 0");
    }

    // Corrutina para disparar el proyectil con retraso
    IEnumerator ShootGun()
    {
        canShoot = false; // Desactivar el disparo temporalmente

        // Crear el proyectil en la posición del launchPoint
        Instantiate(gunPrefab, launchPoint.position, launchPoint.rotation);

        yield return new WaitForSeconds(shootDelay);

        canShoot = true; // Permitir disparar nuevamente
    }
}
