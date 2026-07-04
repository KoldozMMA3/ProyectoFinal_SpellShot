using UnityEngine;
using TMPro;

namespace SpellShot.Gameplay
{
    public class HUDManager : MonoBehaviour
    {
        public static HUDManager Instance { get; private set; }

        [Header("Referencias de UI (TextMeshPro UGUI)")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI targetText;
        
        [Header("Panel de Estado")]
        [SerializeField] private GameObject gameOverPanel; // Casilla para el panel de Game Over

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            UpdateScoreUI(0);
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(false); // Nos aseguramos de que empiece oculto
            }
        }

        public void UpdateScoreUI(int currentScore)
        {
            if (scoreText != null)
            {
                scoreText.text = $"Puntaje: {currentScore}";
            }
        }

        public void UpdateTargetWordUI(string spanishWord)
        {
            if (targetText != null)
            {
                targetText.text = $"Dispara a: <color=yellow>{spanishWord.ToUpper()}</color>";
            }
        }

        /// <summary>
        /// Enciende el letrero visual de Game Over en la pantalla.
        /// </summary>
        public void ShowGameOverUI()
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
        }

        public void RestartGame()
        {
            // 1. Volvemos a activar el tiempo normal del juego
            Time.timeScale = 1f;
            
            // 2. ¡CABLE DE LIMPIEZA!: Le ordenamos al GameManager persistente que borre los puntos y vidas viejos
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ResetGameVariables();
            }
            
            // 3. Volvemos a cargar la escena limpia
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneName);
        }
    }
}