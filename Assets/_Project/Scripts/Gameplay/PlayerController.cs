using UnityEngine;
using UnityEngine.InputSystem;

namespace SpellShot.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Configuración de Movimiento")]
        [SerializeField] private float moveSpeed = 10f;

        [Header("Animaciones (Requerimiento 7)")]
        [SerializeField] private Animator animator; // Referencia al Animator

        private Rigidbody2D rb;
        private Vector2 moveInput;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }

        private void FixedUpdate()
        {
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

            // CONTROL DE ANIMACIÓN IDLE / MOVIMIENTO
            if (animator != null)
            {
                bool isMoving = Mathf.Abs(moveInput.x) > 0.05f;
                animator.SetBool("isMoving", isMoving);
            }
        }

        public void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>();
        }

        /// <summary>
        /// Activa la animación de celebración al subir de nivel o ganar (Requerimiento 7).
        /// </summary>
        public void Celebrate()
        {
            if (animator != null)
            {
                animator.SetTrigger("Celebrate");
            }
        }
    }
}