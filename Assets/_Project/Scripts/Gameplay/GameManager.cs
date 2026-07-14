using UnityEngine;

namespace SpellShot.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Configuración de Partida")]
        [SerializeField] private int currentScore = 0;
        [SerializeField] private int lives = 3; // El jugador arranca con 3 vidas

        [Header("Referencias de FX (Prefabs)")]
        [SerializeField] private GameObject correctParticlesPrefab;
        [SerializeField] private GameObject incorrectParticlesPrefab;

        [Header("Referencias de Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip hitCorrectClip;
        [SerializeField] private AudioClip hitIncorrectClip;

        
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

        /// <summary>
        /// Procesa el impacto de un láser con una palabra validando estrictamente los datos del HUD.
        public void ProcessWordHitWithData(TargetWord struckWord, Vector3 position)
        {
            WaveManager waveInstance = Object.FindFirstObjectByType<WaveManager>();
            if (waveInstance == null) return;

            // Comparamos el ScriptableObject de la palabra golpeada contra el objetivo activo en el HUD
            bool isStrictlyCorrect = struckWord.GetWordData() == waveInstance.CurrentTargetWord;

            if (isStrictlyCorrect)
            {
                currentScore += 10;

                if (HUDManager.Instance != null)
                    HUDManager.Instance.UpdateScoreUI(currentScore);

                // ¡Sincronía total!: Cambiamos al siguiente objetivo al acertar usando el método correcto
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
                    // RETROALIMENTACIÓN DE ERROR (Requerimiento 9)
                    HUDManager.Instance.ShowErrorFeedback(struckWord.GetWordData().wordInEnglish, struckWord.GetWordData().translationToSpanish);
                }

                TakeDamage();

                if (audioSource != null && hitIncorrectClip != null)
                    audioSource.PlayOneShot(hitIncorrectClip);

                if (incorrectParticlesPrefab != null)
                    Instantiate(incorrectParticlesPrefab, position, Quaternion.identity);
            }
        }

        /// <summary>
        /// Resta una vida al jugador y valida el estado de Game Over.
        /// </summary>
        public void TakeDamage()
        {
            lives--;
            Debug.Log($"¡DAÑO DETECTADO! Vidas restantes: {lives}");
            
            // ¡CABLE VISUAL!: Le avisamos al HUD que apague un corazón
            if (HUDManager.Instance != null)
            {
                HUDManager.Instance.UpdateLivesUI(lives);
            }

            if (lives <= 0)
            {
                TriggerGameOver();
            }
        }

        private void TriggerGameOver()
        {
            Debug.LogError("¡GAME OVER! Te has quedado sin vidas.");
            
            // Muestra el letrero visual en la pantalla
            if (HUDManager.Instance != null)
            {
                HUDManager.Instance.ShowGameOverUI();
            }

            Time.timeScale = 0f; // Congela el juego de forma segura
        }

        public void ResetGameVariables()
        {
            currentScore = 0;
            lives = 3; 

            // ¡CABLE VISUAL!: Encendemos todos los corazones al reiniciar la partida
            if (HUDManager.Instance != null)
            {
                HUDManager.Instance.UpdateLivesUI(lives);
            }
        }
    }
}