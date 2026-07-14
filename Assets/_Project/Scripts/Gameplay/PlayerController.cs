using UnityEngine;
using UnityEngine.InputSystem;

namespace SpellShot.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Configuración de Movimiento")]
        [SerializeField] private float moveSpeed = 10f;

        private Rigidbody2D rb;
        private Vector2 moveInput;

        private void Awake()
        {
            // Obtener la referencia del Rigidbody automáticamente
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            // Aplicar el movimiento físico de manera fluida y constante
            MovePlayer();
        }

        /// <summary>
        /// Este método es llamado automáticamente por el componente Player Input 
        /// cuando se presionan las teclas de movimiento (WASD / Flechas / Stick).
        /// </summary>
        public void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>();
        }

        private void MovePlayer()
        {
            // Multiplicamos por fixedDeltaTime para asegurar suavidad independiente de los FPS
            Vector2 targetVelocity = moveInput * moveSpeed;
            rb.linearVelocity = targetVelocity;
        }
    }
}