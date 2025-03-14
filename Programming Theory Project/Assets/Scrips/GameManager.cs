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
    public TMP_Text highScoreText; // Referencia al texto de High Score
    public GameObject resetButton; //reset
    private bool isGameOver = false; // Bandera para controlar el estado del juego
    private int score = 0; // Puntaje inicial
    private int highScore = 0; //record inicial
    private string highScoreName = ""; // Nombre del jugador con récord


    private const string HighScoreKey = "HighScore";
    private const string HighScoreNameKey = "HighScoreName";

    public TMP_Text playerNameText; // Referencia al texto para mostrar el nombre del jugador

    void Start()
    {
        if (resetButton != null)
        {
            resetButton.SetActive(false); // Ocultar el botón Reset al iniciar el juego
        }
        LoadHighScore(); // Cargar el récord desde PlayerPrefs
        UpdateHighScoreUI(); // Actualizar la UI del récord
        UpdatePlayerNameUI(); // Actualizar el texto con el nombre del jugador
    }
    private void UpdatePlayerNameUI()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Anonymous"); // Cargar el nombre del jugador o "Anonymous" por defecto
        if (playerNameText != null)
        {
            playerNameText.text = $"WarShip {playerName}"; // Muestra solo el nombre del jugador
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetHighScore(); // Llama al método para reiniciar el High Score
        }
    }


    // Método para incrementar el puntaje
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();

        // Verifica si se supera el récord
        if (score > highScore)
        {
            highScore = score; // Actualiza el récord
            highScoreName = PlayerPrefs.GetString("PlayerName", "Anonymous"); // Obtiene el nombre actual del jugador
            SaveHighScore(); // Guarda el nuevo récord
            UpdateHighScoreUI(); // Actualiza la UI del récord
        }
    }

    // Método para actualizar la UI de la puntuación
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    // Método para actualizar la UI del High Score
    private void UpdateHighScoreUI()
    {
        if (highScoreText != null)
        {
            highScoreText.text = $"High Score: {highScoreName} {highScore}";
        }
    }

    // Guardar el récord en PlayerPrefs
    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore); // Guardar el récord
        PlayerPrefs.SetString(HighScoreNameKey, highScoreName); // Guardar el nombre del jugador
        PlayerPrefs.Save(); // Asegurar que se guarde
        Debug.Log("Nuevo High Score guardado: " + highScore);
    }

    // Cargar el récord desde PlayerPrefs
    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0); // Cargar el récord (0 por defecto si no existe)
        highScoreName = PlayerPrefs.GetString(HighScoreNameKey, "Anonymous"); // Cargar el nombre (Anonymous por defecto)
        Debug.Log("High Score cargado: " + highScore);
    }

    public void ResetHighScore()
    {
        highScore = 0; // Reinicia el récord a 0
        highScoreName = "Anonymous"; // Reinicia el nombre del jugador
        PlayerPrefs.SetInt("HighScore", highScore); // Guarda el cambio en PlayerPrefs
        PlayerPrefs.SetString(HighScoreNameKey, highScoreName); // Guarda el nombre predeterminado
        PlayerPrefs.Save(); // Asegura que los cambios persistan
        UpdateHighScoreUI(); // Actualiza la interfaz para reflejar el cambio
        Debug.Log("High Score reiniciado.");
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
        SceneManager.LoadScene("Menu"); // Cambia "Menu" al nombre exacto de tu escena del menú principal
    }

    // Salir del juego (solo en plataformas ejecutables)
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego..."); // Depuración
        Application.Quit(); // Cierra el juego
    }
}
