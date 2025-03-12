using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI; // Referencia al panel de Game Over en la UI
    public MonoBehaviour EnemySpawner; // Referencia al script EnemySpawner
    public MonoBehaviour ItemSpawner; // Referencia al script ItemSpawner
    public TMP_Text scoreText; // Referencia al texto de la puntuación
    public GameObject resetButton; //reset
    private bool isGameOver = false; // Bandera para controlar el estado del juego
    private int score = 0; // Puntaje inicial


    void Start()
    {
        if (resetButton != null)
        {
            resetButton.SetActive(false); // Ocultar el botón Reset al iniciar el juego
        }
    }


    // Método para incrementar el puntaje
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    // Método para actualizar la UI de la puntuación
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    // Método para activar el estado de Game Over
    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Debug.Log("¡Game Over!"); // Mensaje en la consola para depuración
            gameOverUI.SetActive(true); // Activar la interfaz de Game Over


            if (resetButton != null)
            {
                resetButton.SetActive(true); // Activar el botón Reset
            }

                Time.timeScale = 0f; // Pausar el juego
            StopSpawners();
        }
    }
    private void StopSpawners()
    {
        if (EnemySpawner !=null)
        {
            EnemySpawner.enabled = false;
            Debug.Log("EnemySpawner desactivado");
        }
        if (ItemSpawner !=null)
        {
            ItemSpawner.enabled = false;
        }
    }

    // Reiniciar el juego
    public void RestartGame()
    {
        Time.timeScale = 1f; // Restaurar la escala de tiempo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia la escena actual
    }

    // Salir del juego (solo en plataformas ejecutables)
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego..."); // Depuración
        Application.Quit(); // Cierra el juego
    }
}
