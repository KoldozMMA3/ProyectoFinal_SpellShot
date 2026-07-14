using UnityEngine;
using TMPro;
using System.Collections;

namespace SpellShot.Gameplay
{
    public class HUDManager : MonoBehaviour
    {
        public static HUDManager Instance { get; private set; }

        [Header("Textos del HUD en Vivo")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI targetText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI timerText;       // <--- CRONÓMETRO EN VIVO
        [SerializeField] private TextMeshProUGUI hitsCounterText; // <--- CONTADOR DE ACIERTOS

        [Header("Anuncio de Nivel Emergente")]
        [SerializeField] private TextMeshProUGUI levelBannerText;
        [SerializeField] private float bannerDuration = 2.5f;

        [Header("Retroalimentación de Error")]
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private float feedbackDuration = 2.5f;

        [Header("Vidas (Corazones)")]
        [SerializeField] private GameObject[] heartIcons;

        [Header("Paneles de Menú")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject gameWinPanel;         // <--- PANEL DE VICTORIA
        [SerializeField] private TextMeshProUGUI victoryMessageText;// <--- MENSAJE DE VELOCIDAD
        [SerializeField] private TextMeshProUGUI victoryStatsText;  // <--- ESTADÍSTICAS FINALES

        private Coroutine bannerCoroutine;
        private Coroutine feedbackCoroutine;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            // Candados iniciales: Ocultamos paneles al arrancar
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            if (gameWinPanel != null) gameWinPanel.SetActive(false);
            if (feedbackText != null) feedbackText.gameObject.SetActive(false);
            if (levelBannerText != null) levelBannerText.gameObject.SetActive(false);
        }

        public void UpdateScoreUI(int score)
        {
            if (scoreText != null) scoreText.text = $"Puntaje: {score}";
        }

        public void UpdateTargetWordUI(string translation)
        {
            if (targetText != null)
                targetText.text = $"Dispara a:\n<color=#FFD700>{translation.ToUpper()}</color>";
        }

        public void UpdateLevelUI(int level)
        {
            if (levelText != null)
                levelText.text = $"Nivel: {level}";
        }

        public void UpdateTimerUI(float seconds)
        {
            if (timerText != null)
            {
                int mins = Mathf.FloorToInt(seconds / 60F);
                int secs = Mathf.FloorToInt(seconds % 60F);
                timerText.text = $"Tiempo: {mins:00}:{secs:00}";
            }
        }

        public void UpdateHitsUI(int hits)
        {
            if (hitsCounterText != null)
                hitsCounterText.text = $"Aciertos: {hits}";
        }

        public void UpdateLivesUI(int currentLives)
        {
            for (int i = 0; i < heartIcons.Length; i++)
            {
                if (heartIcons[i] != null)
                {
                    heartIcons[i].SetActive(i < currentLives);
                }
            }
        }

        public void ShowLevelAnnouncement(string message)
        {
            if (levelBannerText == null) return;

            if (bannerCoroutine != null) StopCoroutine(bannerCoroutine);
            bannerCoroutine = StartCoroutine(DisplayBannerRoutine(message));
        }

        private IEnumerator DisplayBannerRoutine(string message)
        {
            levelBannerText.text = message;
            levelBannerText.gameObject.SetActive(true);
            yield return new WaitForSeconds(bannerDuration);
            levelBannerText.gameObject.SetActive(false);
        }

        public void ShowErrorFeedback(string englishWord, string spanishTranslation)
        {
            if (feedbackText == null) return;

            if (feedbackCoroutine != null) StopCoroutine(feedbackCoroutine);
            feedbackCoroutine = StartCoroutine(DisplayFeedbackRoutine(
                $"<color=#FF4444>¡ERROR!</color>\n<b>{englishWord.ToUpper()}</b> significa <b><color=#FFD700>{spanishTranslation.ToUpper()}</color></b>"
            ));
        }

        private IEnumerator DisplayFeedbackRoutine(string message)
        {
            feedbackText.text = message;
            feedbackText.gameObject.SetActive(true);
            yield return new WaitForSeconds(feedbackDuration);
            feedbackText.gameObject.SetActive(false);
        }

        public void ShowGameOverUI()
        {
            if (gameOverPanel != null) gameOverPanel.SetActive(true);
        }

        /// <summary>
        /// Muestra el panel de Victoria con el mensaje personalizado de velocidad.
        /// </summary>
        public void ShowGameWinUI(string speedMessage, int totalHits, float totalTime)
        {
            if (gameWinPanel != null)
            {
                gameWinPanel.SetActive(true);

                if (victoryMessageText != null)
                    victoryMessageText.text = speedMessage;

                if (victoryStatsText != null)
                {
                    int mins = Mathf.FloorToInt(totalTime / 60F);
                    int secs = Mathf.FloorToInt(totalTime % 60F);
                    victoryStatsText.text = $"<b>Palabras Acertadas:</b> {totalHits}\n<b>Tiempo Total:</b> {mins:00}:{secs:00}";
                }
            }
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