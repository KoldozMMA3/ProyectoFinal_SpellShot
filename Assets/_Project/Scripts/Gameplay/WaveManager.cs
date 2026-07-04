using System.Collections.Generic;
using UnityEngine;
using SpellShot.Data;

namespace SpellShot.Gameplay
{
    public class WaveManager : MonoBehaviour
    {
        [Header("Referencias de Plantillas")]
        [SerializeField] private GameObject targetWordPrefab;

        [Header("Banco de Datos de Palabras")]
        [SerializeField] private List<WordData> wordBank = new List<WordData>();

        [Header("Configuración de la Oleada")]
        [SerializeField] private float spawnInterval = 3f;
        [SerializeField] private float xSpawnRange = 6f;
        [SerializeField] private float ySpawnPosition = 6f;

        private float spawnTimer;
        private int currentWave = 1;
        private WordData currentTargetWord; 

        // Propiedad pública segura para que el GameManager verifique la palabra activa
        public WordData CurrentTargetWord => currentTargetWord;

        private void Start()
        {
            if (wordBank.Count == 0)
            {
                Debug.LogError("El Banco de Datos de Palabras está vacío. Agrega WordData en el Inspector.");
                return;
            }

            // Selecciona la primera palabra objetivo del juego
            SelectNextTargetWord();
        }

        private void Update()
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                SpawnRandomWord();
                spawnTimer = 0f;
            }
        }

        /// <summary>
        /// Elige una nueva palabra objetivo del banco y actualiza el HUD en perfecta sincronía.
        /// </summary>
        public void SelectNextTargetWord()
        {
            if (wordBank.Count == 0) return;

            // Elegimos una palabra completamente al azar del banco
            int randomIndex = Random.Range(0, wordBank.Count);
            currentTargetWord = wordBank[randomIndex];

            if (currentTargetWord != null && HUDManager.Instance != null)
            {
                HUDManager.Instance.UpdateTargetWordUI(currentTargetWord.translationToSpanish);
            }
        }

        private void SpawnRandomWord()
        {
            if (wordBank.Count == 0 || targetWordPrefab == null) return;

            // Balanceo de Gameplay: 40% de probabilidad de lanzar la palabra correcta, 
            // 60% de lanzar distractores para que el jugador busque en pantalla.
            WordData selectedData = null;
            if (Random.value < 0.4f && currentTargetWord != null)
            {
                selectedData = currentTargetWord;
            }
            else
            {
                selectedData = wordBank[Random.Range(0, wordBank.Count)];
            }

            Vector2 spawnPosition = new Vector2(Random.Range(-xSpawnRange, xSpawnRange), ySpawnPosition);
            GameObject spawnedObject = Instantiate(targetWordPrefab, spawnPosition, Quaternion.identity);

            TargetWord targetWordComponent = spawnedObject.GetComponent<TargetWord>();
            if (targetWordComponent != null)
            {
                targetWordComponent.Setup(selectedData);
            }
        }

        public void NextWave()
        {
            currentWave++;
            spawnInterval = Mathf.Max(1f, spawnInterval - 0.3f);
            SelectNextTargetWord(); 
            Debug.Log($"¡Iniciando Oleada {currentWave}! Intervalo de aparición: {spawnInterval}s");
        }
    }
}