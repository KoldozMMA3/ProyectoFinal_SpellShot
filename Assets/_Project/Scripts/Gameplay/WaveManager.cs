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

        private void Start()
        {
            if (wordBank.Count == 0)
            {
                Debug.LogError("El Banco de Datos de Palabras está vacío. Agrega WordData en el Inspector.");
            }
        }

        private void Update()
        {
            // Sistema de temporizador automático para spawnear palabras
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                SpawnRandomWord();
                spawnTimer = 0f;
            }
        }

        private void SpawnRandomWord()
        {
            if (wordBank.Count == 0 || targetWordPrefab == null) return;

            // 1. Elegir una palabra al azar de nuestro banco de ScriptableObjects
            int randomIndex = Random.Range(0, wordBank.Count);
            WordData selectedData = wordBank[randomIndex];

            // 2. Calcular una posición horizontal aleatoria en la parte superior
            Vector2 spawnPosition = new Vector2(Random.Range(-xSpawnRange, xSpawnRange), ySpawnPosition);

            // 3. Instanciar el Prefab de la palabra en el mundo físico
            GameObject spawnedObject = Instantiate(targetWordPrefab, spawnPosition, Quaternion.identity);

            // 4. Inyectar los datos del ScriptableObject al objeto clonado
            TargetWord targetWordComponent = spawnedObject.GetComponent<TargetWord>();
            if (targetWordComponent != null)
            {
                targetWordComponent.Setup(selectedData);
            }
        }

        /// <summary>
        /// Método profesional para incrementar la dificultad de la oleada (Requerimiento 5).
        /// </summary>
        public void NextWave()
        {
            currentWave++;
            // Incrementa la dificultad reduciendo el tiempo de aparición entre palabras
            spawnInterval = Mathf.Max(1f, spawnInterval - 0.3f);
            Debug.Log($"¡Iniciando Oleada {currentWave}! Intervalo de aparición: {spawnInterval}s");
        }
    }
}