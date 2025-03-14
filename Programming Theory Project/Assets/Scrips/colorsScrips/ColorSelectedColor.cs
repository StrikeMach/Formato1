using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ColorSelectedColor : MonoBehaviour
{
    public static Material SelectedMaterial; // Variable estática para almacenar el material seleccionado

    public Material RedMaterial;   // Arrastra aquí el material rojo en el Inspector
    public Material GreenMaterial; // Arrastra aquí el material verde en el Inspector
    public Material IndigoMaterial;  // Arrastra aquí el material azul en el Inspector
    public Material OrangeMaterial;  // Arrastra aquí el material azul en el Inspector

    public Image colorDisplayImage; // Imagen de UI que mostrará el color seleccionado

    private const string SavedColorKey = "SelectedColor"; // Clave para PlayerPrefs

    public void SelectColor(string colorName)
    {
        // Asignar el material basado en el nombre
        switch (colorName)
        {
            case "Red":
                SelectedMaterial = RedMaterial;
                UpdateColorDisplay(Color.red); // Cambia el color de la imagen
                break;
            case "Green":
                SelectedMaterial = GreenMaterial;
                UpdateColorDisplay(Color.green);
                break;
            case "Indigo":
                SelectedMaterial = IndigoMaterial;
                UpdateColorDisplay(new Color(92f / 255f, 76f / 255f, 255f / 255f)); // Indigo: #5C4CFF
                break;
            case "Orange":
                SelectedMaterial = OrangeMaterial;
                UpdateColorDisplay(new Color(255f / 255f, 63f / 255f, 0f)); // Orange: #FF3F00
                break;
            default:
                Debug.LogError("Color no válido seleccionado");
                break;
        }
        // Si no es un nombre predefinido, intenta aplicarlo como valor hexadecimal
        if (ColorUtility.TryParseHtmlString(colorName, out Color hexColor))
        {
            UpdateColorDisplay(hexColor);
            Debug.Log($"Color aplicado desde valor hexadecimal: {colorName}");
        }
        
    }
    private void UpdateColorDisplay(Color color)
    {
        if (colorDisplayImage != null)
        {
            colorDisplayImage.color = color; // Cambia el color de la imagen
            Debug.Log("Color actualizado en la UI.");
        }
        else
        {
            Debug.LogWarning("No se asignó ninguna UI Image para mostrar el color.");
        }
    }

        public void SaveSelectedColor(string colorName)
    {
        PlayerPrefs.SetString(SavedColorKey, colorName); // Guarda el color seleccionado en PlayerPrefs
        PlayerPrefs.Save();
        Debug.Log($"Color {colorName} guardado en PlayerPrefs.");
    }

    public void LoadSelectedColor()
    {
        if (PlayerPrefs.HasKey(SavedColorKey))
        {
            string loadedColorName = PlayerPrefs.GetString(SavedColorKey);
            SelectColor(loadedColorName); // Selecciona y aplica el color guardado
            Debug.Log($"Color {loadedColorName} cargado desde PlayerPrefs.");
        }
        else
        {
            Debug.LogWarning("No hay un color guardado en PlayerPrefs.");
        }
    }
}