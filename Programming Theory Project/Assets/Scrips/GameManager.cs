using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI; // Referencia al panel de Game Over en la UI
    public MonoBehaviour EnemySpawner; // Referencia al script EnemySpawner
    public MonoBehaviour ItemSpawner; // Referencia al script ItemSpawner
    public TMP_Text scoreText; // Referencia al texto de la puntuaci�n
    public TMP_Text highScoreText; // Referencia al texto de High Score
    public GameObject resetButton; //reset
    private bool isGameOver = false; // Bandera para controlar el estado del juego
    private int score = 0; // Puntaje inicial
    private int highScore = 0; //record inicial
    private string highScoreName = ""; // Nombre del jugador con r�cord


    private const string HighScoreKey = "HighScore";
    private const string HighScoreNameKey = "HighScoreName";

    public TMP_Text playerNameText; // Referencia al texto para mostrar el nombre del jugador

    void Start()
    {
        if (resetButton != null)
        {
            resetButton.SetActive(false); // Ocultar el bot�n Reset al iniciar el juego
        }
        LoadHighScore(); // Cargar el r�cord desde PlayerPrefs
        UpdateHighScoreUI(); // Actualizar la UI del r�cord
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
            ResetHighScore(); // Llama al m�todo para reiniciar el High Score
        }
    }


    // M�todo para incrementar el puntaje
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();

        // Verifica si se supera el r�cord
        if (score > highScore)
        {
            highScore = score; // Actualiza el r�cord
            highScoreName = PlayerPrefs.GetString("PlayerName", "Anonymous"); // Obtiene el nombre actual del jugador
            SaveHighScore(); // Guarda el nuevo r�cord
            UpdateHighScoreUI(); // Actualiza la UI del r�cord
        }
    }

    // M�todo para actualizar la UI de la puntuaci�n
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    // M�todo para actualizar la UI del High Score
    private void UpdateHighScoreUI()
    {
        if (highScoreText != null)
        {
            highScoreText.text = $"High Score: {highScoreName} {highScore}";
        }
    }

    // Guardar el r�cord en PlayerPrefs
    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore); // Guardar el r�cord
        PlayerPrefs.SetString(HighScoreNameKey, highScoreName); // Guardar el nombre del jugador
        PlayerPrefs.Save(); // Asegurar que se guarde
        Debug.Log("Nuevo High Score guardado: " + highScore);
    }

    // Cargar el r�cord desde PlayerPrefs
    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0); // Cargar el r�cord (0 por defecto si no existe)
        highScoreName = PlayerPrefs.GetString(HighScoreNameKey, "Anonymous"); // Cargar el nombre (Anonymous por defecto)
        Debug.Log("High Score cargado: " + highScore);
    }

    public void ResetHighScore()
    {
        highScore = 0; // Reinicia el r�cord a 0
        highScoreName = "Anonymous"; // Reinicia el nombre del jugador
        PlayerPrefs.SetInt("HighScore", highScore); // Guarda el cambio en PlayerPrefs
        PlayerPrefs.SetString(HighScoreNameKey, highScoreName); // Guarda el nombre predeterminado
        PlayerPrefs.Save(); // Asegura que los cambios persistan
        UpdateHighScoreUI(); // Actualiza la interfaz para reflejar el cambio
        Debug.Log("High Score reiniciado.");
    }


    // M�todo para activar el estado de Game Over
    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Debug.Log("�Game Over!"); // Mensaje en la consola para depuraci�n
            gameOverUI.SetActive(true); // Activar la interfaz de Game Over


            if (resetButton != null)
            {
                resetButton.SetActive(true); // Activar el bot�n Reset
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
        SceneManager.LoadScene("Menu"); // Cambia "Menu" al nombre exacto de tu escena del men� principal
    }

    // Salir del juego (solo en plataformas ejecutables)
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego..."); // Depuraci�n
        Application.Quit(); // Cierra el juego
    }
}
