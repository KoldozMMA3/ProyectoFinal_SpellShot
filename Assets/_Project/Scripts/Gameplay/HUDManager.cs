using UnityEngine;
using TMPro;
using System.Collections;

namespace SpellShot.Gameplay
{
    public class HUDManager : MonoBehaviour
    {
        public static HUDManager Instance { get; private set; }

        [Header("Textos del HUD")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI targetText;
        [SerializeField] private TextMeshProUGUI levelText;

        [Header("Retroalimentación de Error (Requerimiento 9)")]
        [SerializeField] private TextMeshProUGUI feedbackText; // Texto central emergente
        [SerializeField] private float feedbackDuration = 2.5f;

        [Header("Vidas (Corazones)")]
        [SerializeField] private GameObject[] heartIcons;

        [Header("Paneles de Menú")]
        [SerializeField] private GameObject gameOverPanel;

        private Coroutine feedbackCoroutine;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            if (feedbackText != null) feedbackText.gameObject.SetActive(false);
        }

        public void UpdateScoreUI(int score) => scoreText.text = $"Puntaje: {score}";
        
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

        /// <summary>
        /// Muestra en pantalla el significado de la palabra al cometer un error (Requerimiento 9).
        /// </summary>
        public void ShowErrorFeedback(string englishWord, string spanishTranslation)
        {
            if (feedbackText == null) return;

            if (feedbackCoroutine != null)
                StopCoroutine(feedbackCoroutine);

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