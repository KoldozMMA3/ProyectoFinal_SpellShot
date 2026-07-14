using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using SpellShot.Data;

namespace SpellShot.Gameplay
{
    public class WaveManager : MonoBehaviour
    {
        [Header("Banco de Datos")]
        [SerializeField] private List<WordData> wordBank = new List<WordData>();

        [Header("Configuración de Oleada")]
        [SerializeField] private GameObject targetWordPrefab;
        [SerializeField] private float spawnInterval = 3f;
        [SerializeField] private float xSpawnRange = 30f;
        [SerializeField] private float ySpawnPosition = 6f;

        private WordData currentTargetWord;
        private float spawnTimer = 0f;
        private int currentLevelCategory = 1;

        public WordData CurrentTargetWord => currentTargetWord;

        private void Start()
        {
            if (wordBank.Count == 0) return;
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
        /// Cambia la dificultad y exige palabras EXCLUSIVAS del nuevo Nivel.
        /// </summary>
        public void SetLevelDifficulty(int level)
        {
            currentLevelCategory = level;

            switch (level)
            {
                case 1:
                    spawnInterval = 1.5f;
                    break;
                case 2:
                    spawnInterval = 0.8f; // Más rápido
                    break;
                case 3:
                    spawnInterval = 0.2f; // Ritmo ágil para el Nivel Final
                    break;
            }

            // ¡Sincronía total!: Forzamos el cambio inmediato a un objetivo del NUEVO nivel
            SelectNextTargetWord();

            Debug.Log($"WaveManager ajustado EXCLUSIVAMENTE a Nivel {level}. Intervalo: {spawnInterval}s");
        }

        public void SelectNextTargetWord()
        {
            List<WordData> availableWords = GetFilteredWords();

            int randomIndex = Random.Range(0, availableWords.Count);
            currentTargetWord = availableWords[randomIndex];

            if (currentTargetWord != null && HUDManager.Instance != null)
            {
                HUDManager.Instance.UpdateTargetWordUI(currentTargetWord.translationToSpanish);
            }
        }

        private void SpawnRandomWord()
        {
            List<WordData> availableWords = GetFilteredWords();

            // 40% probabilidad de generar la palabra objetivo activa del HUD, 60% distractores del MISMO NIVEL
            WordData selectedData = (Random.value < 0.4f && currentTargetWord != null)
                ? currentTargetWord
                : availableWords[Random.Range(0, availableWords.Count)];

            Vector2 spawnPosition = new Vector2(Random.Range(-xSpawnRange, xSpawnRange), ySpawnPosition);
            GameObject spawnedObject = Instantiate(targetWordPrefab, spawnPosition, Quaternion.identity);

            TargetWord targetWordComponent = spawnedObject.GetComponent<TargetWord>();
            if (targetWordComponent != null)
            {
                targetWordComponent.Setup(selectedData);
            }
        }

        /// <summary>
        /// Filtra el banco para obtener ÚNICAMENTE las palabras del nivel actual.
        /// </summary>
        private List<WordData> GetFilteredWords()
        {
            // FILTRADO ESTRICTO (==): Solo palabras asignadas a este nivel específico
            List<WordData> filtered = wordBank.Where(w => w != null && w.levelCategory == currentLevelCategory).ToList();

            // Failsafe: Si no hay palabras configuradas para este nivel en el banco, usa todo el banco para no congelar el juego
            if (filtered.Count == 0)
            {
                Debug.LogWarning($"No hay palabras con Level Category = {currentLevelCategory}. Usando todo el banco.");
                return wordBank;
            }

            return filtered;
        }
    }
}