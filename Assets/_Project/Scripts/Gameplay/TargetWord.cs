using UnityEngine;
using TMPro;
using SpellShot.Data;

namespace SpellShot.Gameplay
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class TargetWord : MonoBehaviour
    {
        [Header("Configuración de Movimiento")]
        [SerializeField] private float fallSpeed = 2f;

        [Header("Referencias de UI")]
        [SerializeField] private TextMeshPro textMesh;

        private WordData wordData;
        private Rigidbody2D rb;
        private BoxCollider2D boxCollider;
        private bool hasCrossedLine = false; 

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();

            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        public void Setup(WordData data)
        {
            wordData = data;
            if (textMesh != null && wordData != null)
            {
                textMesh.text = wordData.wordInEnglish;
            }

            if (rb != null)
            {
                rb.linearVelocity = Vector2.down * fallSpeed;
            }
        }

        public WordData GetWordData() => wordData;

        /// <summary>
        /// EFECTO FANTASMA: Desactiva colisiones y oculta la palabra inmediatamente 
        /// para que no interfiera en lo absoluto debajo de la barra.
        /// </summary>
        public void MarkAsCrossed()
        {
            if (hasCrossedLine) return;
            hasCrossedLine = true;

            // 1. Apagamos el colisionador para que ningún láser lo pueda tocar
            if (boxCollider != null) boxCollider.enabled = false;

            // 2. Detenemos la física por completo
            if (rb != null) rb.linearVelocity = Vector2.zero;

            // 3. Lo removemos del flujo visual y de colisiones del Canvas de inmediato
            gameObject.SetActive(false); 
        }

        private void Update()
        {
            // CANDADO DE SEGURIDAD (Failsafe): Si por un parpadeo del motor la palabra pasa de largo
            // y llega al subsuelo profundo sin tocar el trigger, se vuelve fantasma y se borra sola.
            if (transform.position.y < -5.0f && !hasCrossedLine)
            {
                MarkAsCrossed();
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Si el candado está activo, ignoramos cualquier choque de inmediato
            if (hasCrossedLine) return;

            if (collision.gameObject.name.Contains("LaserPrefab") || collision.GetComponent<Projectile>() != null)
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ProcessWordHitWithData(this, transform.position);
                }

                Destroy(collision.gameObject); // Destruye el láser
                Destroy(gameObject);           // Destruye la palabra
            }
        }
    }
}