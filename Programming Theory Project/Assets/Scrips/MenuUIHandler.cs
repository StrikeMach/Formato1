using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necesario para trabajar con UI Image
public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private Image colorPreview; // Un preview del color seleccionado

    [SerializeField] private TMP_InputField nameInputField; // Campo para ingresar el nombre del jugador
    [SerializeField] private TextMeshProUGUI highScoreText; // UI Text para la puntuación más alta

    [SerializeField] private ColorSelectedColor colorManager; // Referencia al script ColorSelectedColor

    [SerializeField] private GameObject highScoreDisplay; // Referencia al texto UI o panel que mostrará el High Score

    private const string HighScoreKey = "HighScore"; // Clave para la puntuación más alta
    private const string HighScoreNameKey = "HighScoreName"; // Clave para el nombre del jugador con la puntuación más alta
    private const string PlayerNameKey = "PlayerName"; // Clave para el nombre del jugador actual


    private const string SavedColorKey = "SavedColor"; // Clave para PlayerPrefs

    private void Start()
    {
        // Desactivar el panel o texto al iniciar
        if (highScoreDisplay != null)
        {
            highScoreDisplay.SetActive(false);
        }



        // Cargar y aplicar automáticamente el color guardado, si existe
        if (PlayerPrefs.HasKey(SavedColorKey))
        {
            string loadedColorHex = PlayerPrefs.GetString(SavedColorKey);
            if (ColorUtility.TryParseHtmlString(loadedColorHex, out Color color))
            {
                colorPreview.color = color; // Aplica el color al objeto preview
                Debug.Log($"Color inicial {loadedColorHex} cargado al iniciar.");
            }
        }
        else
        {
            Debug.LogWarning("No hay un color guardado para cargar al iniciar.");
        }
        
    }
    // Método para seleccionar un color desde los botones
    public void SelectColor(string colorHex)
    {
        if (ColorUtility.TryParseHtmlString(colorHex, out Color color))
        {
            colorPreview.color = color; // Actualiza el color del objeto preview
            Debug.Log($"Color seleccionado: {colorHex}");
        }
        else
        {
            Debug.LogWarning("El formato del color no es válido. Usa un código hexadecimal.");
        }
    }
    public void SaveName()
    {
        if (!string.IsNullOrEmpty(nameInputField.text))
        {
            PlayerPrefs.SetString(PlayerNameKey, nameInputField.text); // Guarda el nombre en PlayerPrefs
            PlayerPrefs.Save(); // Asegura que los datos se guarden
            Debug.Log($"Nombre guardado: {nameInputField.text}");
        }
        else
        {
            Debug.LogWarning("El campo de nombre está vacío. Por favor, ingresa un nombre.");
        }
    }


    // Método para cargar el nombre guardado
    public void LoadName()
    {
        if (PlayerPrefs.HasKey(PlayerNameKey))
        {
            string loadedName = PlayerPrefs.GetString(PlayerNameKey); // Carga el nombre desde PlayerPrefs
            nameInputField.text = loadedName; // Rellena el campo de entrada con el nombre cargado
            Debug.Log($"Nombre cargado: {loadedName}");
        }
        else
        {
            Debug.LogWarning("No hay ningún nombre guardado. Por favor, guarda un nombre primero.");
        }
    }
    public void StartButton()
    {
        // Guarda el nombre ingresado antes de iniciar el juego
        if (!string.IsNullOrEmpty(nameInputField.text))
        {
            PlayerPrefs.SetString("PlayerName", nameInputField.text);
            PlayerPrefs.Save();
            Debug.Log($"Nombre del jugador guardado: {nameInputField.text}");
        }
        else
        {
            Debug.LogWarning("El campo de nombre está vacío. Por favor ingresa tu nombre.");
        }
        // Carga el siguiente escenario
        SceneManager.LoadScene("SampleScene"); // Cambia "NombreDelSiguienteEscenario" al nombre real de tu escena.
    }

    public void ExitButton()
    {

        Application.Quit();
    }
    // Método para guardar el color seleccionado
    public void SaveColor()
    {
        Color currentColor = colorPreview.color; // Obtén el color actual de la imagen
        string colorHex = ColorUtility.ToHtmlStringRGB(currentColor); // Convierte a formato hexadecimal
        PlayerPrefs.SetString(SavedColorKey, $"#{colorHex}"); // Guarda el color en formato hexadecimal
        PlayerPrefs.Save();
        Debug.Log($"Color guardado: #{colorHex}");
    }


    // Método para cargar el color guardado
    public void LoadColor()
    {
        if (PlayerPrefs.HasKey(SavedColorKey))
        {
            string loadedColorHex = PlayerPrefs.GetString(SavedColorKey); // Recupera el color desde PlayerPrefs
            if (!string.IsNullOrEmpty(loadedColorHex) && ColorUtility.TryParseHtmlString(loadedColorHex, out Color color))
            {
                colorPreview.color = color; // Aplica el color a la vista previa
                Debug.Log($"Color cargado: {loadedColorHex}");
            }
            else
            {
                Debug.LogWarning("El color cargado es nulo o inválido.");
            }
        }
        else
        {
            Debug.LogWarning("No hay un color guardado. Guarda un color primero.");
        }
    }



    private bool IsValidHexColor(string colorHex)
    {
        return ColorUtility.TryParseHtmlString(colorHex, out _);
    }

    // Método para mostrar el High Score en el UI
    public void ShowHighScore()
    {
        if (highScoreDisplay != null)
        {
            highScoreDisplay.SetActive(true); // Activa el panel o texto
        }

        // Asegurarse de que los valores se carguen correctamente desde PlayerPrefs
        if (PlayerPrefs.HasKey(HighScoreKey) && PlayerPrefs.HasKey(HighScoreNameKey))
        {
            int highScore = PlayerPrefs.GetInt(HighScoreKey);
            string highScoreName = PlayerPrefs.GetString(HighScoreNameKey);

            // Actualiza el texto en el panel del High Score Display
            TextMeshProUGUI displayText = highScoreDisplay.GetComponent<TextMeshProUGUI>();
            if (displayText != null)
            {
                displayText.text = $"High Score: {highScoreName} - {highScore}";
            }

            Debug.Log($"Mostrando High Score: {highScoreName} - {highScore}");
        }
        else
        {
            Debug.LogWarning("No hay un High Score guardado.");
        }
    }

    // Otros métodos anteriores (SaveColor, LoadColor, etc.) permanecen igual...
}
