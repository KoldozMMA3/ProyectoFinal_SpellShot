using UnityEngine;
using UnityEngine.InputSystem;

namespace SpellShot.Gameplay
{
    public class PlayerShooting : MonoBehaviour
    {
        [Header("Referencias de Disparo")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform firePoint;

        /// <summary>
        /// Cambiado de OnFire a OnAttack para sincronizarse perfectamente con 
        /// el nuevo asset 'InputSystem_Actions' de Unity.
        /// </summary>
        public void OnAttack(InputValue value)
        {
            // Este log aparecerá en tu consola cada vez que uses el Clic Izquierdo o la Barra Espaciadora
            Debug.Log($"¡Input de disparo detectado mediante OnAttack! Presionado: {value.isPressed}");

            if (value.isPressed)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            if (projectilePrefab != null && firePoint != null)
            {
                // Instancia el láser en el punto de disparo
                Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            }
            else
            {
                Debug.LogWarning("Asegúrate de arrastrar el LaserPrefab y el FirePoint al Inspector del Player.");
            }
        }
    }
}