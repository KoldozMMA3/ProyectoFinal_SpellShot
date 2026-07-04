using UnityEngine;

namespace SpellShot.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        // Instancia estática global (Patrón Singleton)
        public static GameManager Instance { get; private set; }

        [Header("Efectos de Partículas (Requerimiento 3)")]
        [SerializeField] private GameObject correctParticlesPrefab;
        [SerializeField] private GameObject incorrectParticlesPrefab;

        [Header("Efectos de Sonido (Requerimiento 6)")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip hitCorrectClip;
        [SerializeField] private AudioClip hitIncorrectClip;

        // Registro de puntuación acumulada (Requerimiento 8)
        private int currentScore = 0;

        private void Awake()
        {
            // Garantizar que solo exista un GameManager en escena
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
        /// Procesa el impacto de una palabra evaluando si es un acierto o error de forma profesional.
        /// </summary>
        public void ProcessWordHit(bool isCorrect, Vector3 spawnPosition)
        {
            if (isCorrect)
            {
                currentScore += 10;
                Debug.Log($"¡Palabra Correcta! Puntuación: {currentScore}");

                // Retroalimentación Visual Verde
                if (correctParticlesPrefab != null)
                    Instantiate(correctParticlesPrefab, spawnPosition, Quaternion.identity);

                // Retroalimentación Sonora de Acierto
                if (audioSource != null && hitCorrectClip != null)
                    audioSource.PlayOneShot(hitCorrectClip);
            }
            else
            {
                currentScore = Mathf.Max(0, currentScore - 5); // Evita puntajes negativos
                Debug.Log($"¡Palabra Incorrecta! Puntuación: {currentScore}");

                // Retroalimentación Visual Roja
                if (incorrectParticlesPrefab != null)
                    Instantiate(incorrectParticlesPrefab, spawnPosition, Quaternion.identity);

                // Retroalimentación Sonora de Error
                if (audioSource != null && hitIncorrectClip != null)
                    audioSource.PlayOneShot(hitIncorrectClip);
            }
        }

        // Método público para leer el puntaje desde el HUD más adelante
        public int GetScore() => currentScore;
    }
}