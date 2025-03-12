using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void DestroyedAction();
    public event DestroyedAction OnDestroyed;

    public float speed;
    public int health = 1;
    public int scoreValue = 10; // Puntos que otorga este enemigo al ser destruido
    private float Ranget = 57;
    private bool isGameOver = false;
    private Rigidbody enemyRb;
    private GameObject Player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        Player = GameObject.Find("Player");

    }

    // Update is called once per frame

    void Update()
    {
        if (isGameOver) return; // Detener el comportamiento del enemigo si es Game Over

        // Dirección hacia el jugador
        Vector3 direction = (Player.transform.position - transform.position).normalized;

        // Rotación y movimiento hacia el jugador
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(270f, targetRotation.eulerAngles.y, 0), Time.deltaTime * 10f);
        enemyRb.AddForce(new Vector3(direction.x, 0, direction.z) * speed);

        // Limitar posición dentro del rango
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -Ranget, Ranget),
            transform.position.y,
            Mathf.Clamp(transform.position.z, -Ranget, Ranget)
        );

    }
    // Pausar el enemigo cuando sea Game Over
    public void FreezeEnemy()
    {
        isGameOver = true;
        enemyRb.Sleep(); // Detener el movimiento físico
        enemyRb.isKinematic = true; // Congelar las físicas del Rigidbody
    }


    // Detecta colisiones
    void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto con el que colisiona es "gun"
        if (collision.gameObject.CompareTag("Gun"))
        {
            health--; // Reduce la vida en 1
            // Si la vida llega a cero, destruye al enemigo
            if (health <= 0)
            {
                Debug.Log("Enemigo destruido por Gun");
                OnDestroyed?.Invoke();

                // Incrementar el puntaje
                GameObject gameManagerObj = GameObject.Find("GameManager");
                if (gameManagerObj != null)
                {
                    GameManager gameManager = gameManagerObj.GetComponent<GameManager>();
                    gameManager.AddScore(scoreValue); // Usa el valor de scoreValue para sumar puntos
                }


                    Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Player")) // Cuando colisiona con el jugador
        {
            Debug.Log("Enemigo destruido por Player");
            OnDestroyed?.Invoke(); // Notificar al Spawner

            Destroy(gameObject); // Destruir enemigo
        }
    }
}