using UnityEngine;

namespace SpellShot.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        [Header("Configuración del Proyectil")]
        [SerializeField] private float speed = 12f;
        [SerializeField] private float lifeTime = 3f;

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            // Hace que la bala se mueva hacia arriba (el eje Y positivo del objeto)
            rb.linearVelocity = transform.up * speed;
            
            // Optimización profesional: destruye la bala automáticamente tras 'lifeTime' segundos
            Destroy(gameObject, lifeTime);
        }
    }
}