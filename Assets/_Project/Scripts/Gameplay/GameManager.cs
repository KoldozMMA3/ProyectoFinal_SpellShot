using UnityEngine;

namespace SpellShot.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Configuración de Partida")]
        [SerializeField] private int currentScore = 0;
        [SerializeField] private int lives = 3;
        [SerializeField] private int currentLevel = 1;
        [SerializeField] private int totalCorrectHits = 0; // Contador total de aciertos

        [Header("Cronómetro de Juego")]
        [SerializeField] private float gameTimer = 0f;
        [SerializeField] private float fastTimeThreshold = 90f; // Menos de 90 segundos = súper veloz

        [Header("Meta de Victoria")]
        [SerializeField] private int winScoreThreshold = 200; // Puntos requeridos para ganar el juego

        [Header("Umbrales de Puntuación por Nivel")]
        [SerializeField] private int level2ScoreThreshold = 100;
        [SerializeField] private int level3ScoreThreshold = 150;

        [Header("Referencias de FX (Prefabs)")]
        [SerializeField] private GameObject correctParticlesPrefab;
        [SerializeField] private GameObject incorrectParticlesPrefab;

        [Header("Referencias de Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip hitCorrectClip;
        [SerializeField] private AudioClip hitIncorrectClip;

        private bool isGameOverOrWon = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (HUDManager.Instance != null)
            {
                HUDManager.Instance.UpdateLevelUI(currentLevel);
                HUDManager.Instance.UpdateHitsUI(totalCorrectHits);
                HUDManager.Instance.ShowLevelAnnouncement("¡NIVEL 1!");
            }
        }

        private void Update()
        {
            // Acumula tiempo en vivo si la partida está activa
            if (!isGameOverOrWon && Time.timeScale > 0f)
            {
                gameTimer += Time.deltaTime;

                if (HUDManager.Instance != null)
                {
                    HUDManager.Instance.UpdateTimerUI(gameTimer);
                }
            }
        }

        public void ProcessWordHitWithData(TargetWord struckWord, Vector3 position)
        {
            if (isGameOverOrWon) return;

            WaveManager waveInstance = Object.FindFirstObjectByType<WaveManager>();
            if (waveInstance == null) return;

            bool isStrictlyCorrect = struckWord.GetWordData() == waveInstance.CurrentTargetWord;

            if (isStrictlyCorrect)
            {
                currentScore += 10;
                totalCorrectHits++; // Incrementa contador de aciertos

                if (HUDManager.Instance != null)
                {
                    HUDManager.Instance.UpdateScoreUI(currentScore);
                    HUDManager.Instance.UpdateHitsUI(totalCorrectHits);
                }

                // ¿ALCANZÓ LA META FINAL DEL JUEGO?
                if (currentScore >= winScoreThreshold)
                {
                    TriggerGameWin();
                    return;
                }

                // Evalúa paso a Nivel 2 o 3
                CheckLevelProgression(waveInstance);

                waveInstance.SelectNextTargetWord();

                if (audioSource != null && hitCorrectClip != null)
                    audioSource.PlayOneShot(hitCorrectClip);

                if (correctParticlesPrefab != null)
                    Instantiate(correctParticlesPrefab, position, Quaternion.identity);
            }
            else
            {
                currentScore = Mathf.Max(0, currentScore - 5);

                if (HUDManager.Instance != null)
                {
                    HUDManager.Instance.UpdateScoreUI(currentScore);
                    HUDManager.Instance.ShowErrorFeedback(struckWord.GetWordData().wordInEnglish, struckWord.GetWordData().translationToSpanish);
                }

                TakeDamage();

                if (audioSource != null && hitIncorrectClip != null)
                    audioSource.PlayOneShot(hitIncorrectClip);

                if (incorrectParticlesPrefab != null)
                    Instantiate(incorrectParticlesPrefab, position, Quaternion.identity);
            }
        }

        private void CheckLevelProgression(WaveManager waveInstance)
        {
            if (currentScore >= level3ScoreThreshold && currentLevel < 3)
            {
                SetLevel(3, "¡NIVEL 3 - NIVEL FINAL!", waveInstance);
            }
            else if (currentScore >= level2ScoreThreshold && currentLevel < 2)
            {
                SetLevel(2, "¡NIVEL 2!", waveInstance);
            }
        }

        private void SetLevel(int newLevel, string levelAnnouncement, WaveManager waveInstance)
        {
            currentLevel = newLevel;

            if (HUDManager.Instance != null)
            {
                HUDManager.Instance.UpdateLevelUI(currentLevel);
                HUDManager.Instance.ShowLevelAnnouncement(levelAnnouncement);
            }

            if (waveInstance != null)
            {
                waveInstance.SetLevelDifficulty(currentLevel);
            }
        }

        /// <summary>
        /// Evalúa la velocidad al llegar a la meta y dispara la pantalla de Victoria.
        /// </summary>
        private void TriggerGameWin()
        {
            isGameOverOrWon = true;
            Time.timeScale = 0f; // Congela el juego

            string message;
            if (gameTimer <= fastTimeThreshold)
            {
                message = "<color=#00FF00>¡FELICIDADES ACABASTE EL JUEGO!</color>\n<color=#FFD700>¡Eres súper veloz! 🚀</color>";
            }
            else
            {
                message = "<color=#FF8800>¡Completaste la meta!</color>\nPero eres súper lento 🐢.\n<i>Mejora tu nivel de inglés o muévete estratégicamente.</i>";
            }

            if (HUDManager.Instance != null)
            {
                HUDManager.Instance.ShowGameWinUI(message, totalCorrectHits, gameTimer);
            }
        }

        public void TakeDamage()
        {
            lives--;

            if (HUDManager.Instance != null)
                HUDManager.Instance.UpdateLivesUI(lives);

            if (lives <= 0)
            {
                TriggerGameOver();
            }
        }

        private void TriggerGameOver()
        {
            isGameOverOrWon = true;

            if (HUDManager.Instance != null)
                HUDManager.Instance.ShowGameOverUI();

            Time.timeScale = 0f;
        }

        public void ResetGameVariables()
        {
            currentScore = 0;
            lives = 3;
            currentLevel = 1;
            totalCorrectHits = 0;
            gameTimer = 0f;
            isGameOverOrWon = false;

            if (HUDManager.Instance != null)
            {
                HUDManager.Instance.UpdateScoreUI(currentScore);
                HUDManager.Instance.UpdateLevelUI(currentLevel);
                HUDManager.Instance.UpdateLivesUI(lives);
                HUDManager.Instance.UpdateHitsUI(totalCorrectHits);
                HUDManager.Instance.UpdateTimerUI(gameTimer);
                HUDManager.Instance.ShowLevelAnnouncement("¡NIVEL 1!");
            }

            WaveManager waveInstance = Object.FindFirstObjectByType<WaveManager>();
            if (waveInstance != null)
            {
                waveInstance.SetLevelDifficulty(1);
            }
        }
    }
}