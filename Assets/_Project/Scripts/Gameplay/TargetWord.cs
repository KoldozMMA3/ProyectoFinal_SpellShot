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

        private WordData wordData;

        public void Setup(WordData data)
        {
            wordData = data;
            
            if (textMesh != null && wordData != null)
            {
                textMesh.text = wordData.wordInEnglish;
            }
        }

        public bool IsCorrect() => wordData != null && wordData.isCorrectTarget;
        public string GetTranslation() => wordData != null ? wordData.translationToSpanish : "";

        private void Update()
        {
            // Mueve el bloque completo
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

            // Cambiado de -6f a -16f para adaptarse al nuevo zoom de la Main Camera
            if (transform.position.y < -16f)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // En cuanto el láser dinámico toque este colisionador, se activará el impacto
            if (collision.gameObject.name.Contains("LaserPrefab") || collision.GetComponent<Projectile>() != null)
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ProcessWordHit(IsCorrect(), transform.position);
                }

                Destroy(collision.gameObject); // Destruye el láser
                Destroy(gameObject);           // Destruye la palabra
            }
        }
    }
}