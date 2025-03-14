using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject lifeIconPrefab; // Prefab del ícono de vida
    public Transform lifeBarContainer; // Contenedor de la barra de vida
    private float Range = 57;
    public float speed = 5.0f;
    public float speedRotate = 8.0f;


    //Temporisador de velocidad
    public int health = 5; // Vida inicial del jugador
    public GameManager gameManager; // Referencia al GameManager

    private float boostedSpeed = 0f;

    // Configuración de disparo
    public GameObject gunPrefab; // Prefab del proyectil
    public Transform launchPoint; // Punto desde donde se lanza el proyectil
    private bool canShoot = true; //control de disparo
    private float shootDelay = 0.3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Aplica el material seleccionado al jugador
        ApplySelectedColor();

        InitializeLifeBar();
    }

    // Método para aplicar el material seleccionado al jugador
    private void ApplySelectedColor()
    {
        if (ColorSelectedColor.SelectedMaterial != null)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = ColorSelectedColor.SelectedMaterial;
                Debug.Log("Material aplicado al jugador: " + ColorSelectedColor.SelectedMaterial.name);
            }
            else
            {
                Debug.LogWarning("No se encontró un Renderer en el jugador.");
            }
        }
        else
        {
            Debug.LogWarning("No se seleccionó ningún material. Usando el material predeterminado.");
        }
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
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Iten"))
        {
            Debug.Log("El jugador ha recogido un objeto.");
            //Incrementa la velocidad del jugador
            boostedSpeed = 15f;
            AddLife();
            // Aumenta la velocidad de disparo
            shootDelay = Mathf.Max(0.1f, shootDelay - 0.1f); // Disminuir retraso, pero no menos de 0.1s


            //Destruye el objeto Iten
            Destroy(other.gameObject);
            // Inicia la corrutina para resetear la velocidad
            StartCoroutine(ResetSpeed());
        }
        else if (other.CompareTag("Enemy"))
        {
            Debug.Log("¡El jugador ha sido golpeado por un enemigo!");
            if (health > 0)
            {
                health--; // Reduce la vida
                UpdateLifeBar(); // Actualiza la barra de vida
                Debug.Log($"Vida perdida. Vidas actuales: {health}");
            }


            if (health <= 0)
            {
                Debug.Log("¡Game Over!");
                gameManager.GameOver(); // Llama al Game Over en el GameManager

                Camera.main.transform.SetParent(null);
                FreezePlayer(); // Destruye al jugador
            }
        }
    }
    private void FreezePlayer()
    {
        // Desactiva el movimiento y la capacidad de disparar
        speed = 0f; // Establece la velocidad a 0 para evitar movimiento
        boostedSpeed = 0f; // Detiene cualquier boost de velocidad
        canShoot = false; // Impide disparar

        // Oculta el modelo del jugador
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false; // Desactiva los renderizadores del modelo
        }

        // Opcional: Si quieres congelar al Rigidbody del jugador
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            
                rb.MovePosition(transform.position); // Mantiene al objeto en su posición actual
                rb.angularVelocity = Vector3.zero; // Detiene la rotación
            rb.isKinematic = true; // Congela las físicas
        }

        Debug.Log("Jugador congelado y oculto.");
    }


    // Agregar un nuevo ícono de vida
    private void InitializeLifeBar()
    {
        for (int i = 0; i < health; i++)
        {
            AddLifeIcon();
        }
    }

    // Agrega un nuevo ícono de vida
    private void AddLifeIcon()
    {
        if (lifeIconPrefab != null && lifeBarContainer != null)
        {
            // Calcula la posición local para el nuevo ícono
            int currentLives = lifeBarContainer.childCount; // Número actual de íconos en el contenedor
            Vector3 newPosition = new Vector3(currentLives * 60, 0, 0); // Desplazamiento hacia la derecha

            // Instancia el nuevo ícono y configúralo
            GameObject newLifeIcon = Instantiate(lifeIconPrefab, lifeBarContainer);
            newLifeIcon.transform.localPosition = newPosition; // Posición relativa al contenedor
            newLifeIcon.SetActive(true); // Asegúrate de que el nuevo ícono esté activo
        }
        else
        {
            Debug.LogWarning("lifeIconPrefab o lifeBarContainer no están asignados en el Inspector.");
        }
    }

    private void AddLife()
    {
        if (lifeIconPrefab != null && lifeBarContainer != null)
        {
            if (health < lifeBarContainer.childCount) // Si hay vidas "ocultas" que se pueden recuperar
            {
                // Recupera una vida activando el siguiente ícono oculto
                health++; // Incrementa la vida
                UpdateLifeBar(); // Reactiva el siguiente ícono oculto
                Debug.Log($"Vida recuperada. Vidas actuales: {health}");
            }
            else // Si todos los íconos ya están activos, agrega una nueva vida
            {
                health++; // Incrementa la vida
                AddLifeIcon(); // Agrega un nuevo ícono de vida
                Debug.Log($"Vida extra añadida. Vidas actuales: {health}");
            }
        }
        else
        {
            Debug.LogWarning("lifeIconPrefab o lifeBarContainer no están asignados en el Inspector.");
        }
    }




    // Actualiza la barra de vida
    private void UpdateLifeBar()
    {
        for (int i = 0; i < lifeBarContainer.childCount; i++)
        {
            // Activa los íconos hasta la cantidad de vidas actuales y oculta los demás
            lifeBarContainer.GetChild(i).gameObject.SetActive(i < health);
        }
    }


    //Temporisador
    private IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(10f);
        boostedSpeed = 0f;
        shootDelay = 0.3f;
        Debug.Log("Boost reseteado.");
    }

    private IEnumerator ShootGun()
    {
        canShoot = false;
        Instantiate(gunPrefab, launchPoint.position, launchPoint.rotation);
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;

    }
}
