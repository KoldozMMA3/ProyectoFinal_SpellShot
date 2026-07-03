using UnityEngine;
using TMPro;
using SpellShot.Data;

namespace SpellShot.Gameplay
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TargetWord : MonoBehaviour
    {
        [Header("Configuración de Movimiento")]
        [SerializeField] private float fallSpeed = 2f;

        [Header("Referencias de UI")]
        [SerializeField] private TextMeshPro textMesh;

        // Almacena los datos lingüísticos de esta palabra específica
        private WordData wordData;

        /// <summary>
        /// Método profesional para inicializar dinámicamente el objetivo desde el Spawner.
        /// </summary>
        public void Setup(WordData data)
        {
            wordData = data;
            
            if (textMesh != null && wordData != null)
            {
                // Asigna el texto en inglés al componente visual
                textMesh.text = wordData.wordInEnglish;
            }
        }

        // Retorna si esta palabra es el objetivo correcto de la oleada
        public bool IsCorrect() => wordData != null && wordData.isCorrectTarget;

        // Retorna la traducción para la retroalimentación en caso de error
        public string GetTranslation() => wordData != null ? wordData.translationToSpanish : "";

        private void Update()
        {
            // Hace que la palabra descienda constantemente de forma suave
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

            // Optimización: Si la palabra sale de la pantalla por abajo, se destruye
            if (transform.position.y < -6f)
            {
                Destroy(gameObject);
            }
        }
    }
}