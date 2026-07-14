using UnityEngine;
using TMPro;

namespace SpellShot.Gameplay
{
    public class HUDManager : MonoBehaviour
    {
        public static HUDManager Instance { get; private set; }

        [Header("Textos del HUD")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI targetText;

        [Header("Vidas (Corazones)")]
        [SerializeField] private GameObject[] heartIcons; // Arreglo para guardar los 3 iconos

        [Header("Paneles de Menú")]
        [SerializeField] private GameObject gameOverPanel;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            // CANDADO INICIAL: Apaga el panel de Game Over al arrancar la escena
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(false);
            }
        }

        public void UpdateScoreUI(int score) => scoreText.text = $"Puntaje: {score}";
        
        public void UpdateTargetWordUI(string translation) => targetText.text = $"Dispara a:\n<color=#FFD700>{translation.ToUpper()}</color>";

        /// <summary>
        /// Apaga o enciende los iconos de corazones según las vidas restantes.
        /// </summary>
        public void UpdateLivesUI(int currentLives)
        {
            for (int i = 0; i < heartIcons.Length; i++)
            {
                if (heartIcons[i] != null)
                {
                    // Si el índice del corazón es menor a las vidas actuales, se enciende. Si no, se apaga.
                    heartIcons[i].SetActive(i < currentLives);
                }
            }
        }

        public void ShowGameOverUI()
        {
            if (gameOverPanel != null) gameOverPanel.SetActive(true);
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            
            if (GameManager.Instance != null)
                GameManager.Instance.ResetGameVariables();
            
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneName);
        }
    }
}