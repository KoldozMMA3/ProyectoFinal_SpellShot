using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpellShot.Gameplay
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("Configuración de Navegación")]
        [Tooltip("Nombre exacto de la escena de juego principal.")]
        [SerializeField] private string gameSceneName = "SampleScene";

        /// <summary>
        /// Método llamado al hacer clic en 'COMENZAR JUEGO'.
        /// </summary>
        public void StartGame()
        {
            Time.timeScale = 1f; // Asegura que el tiempo corra normal
            SceneManager.LoadScene(gameSceneName);
        }

        /// <summary>
        /// Método para cerrar la aplicación.
        /// </summary>
        public void QuitGame()
        {
            Debug.Log("Saliendo del juego...");
            Application.Quit();
        }
    }
}