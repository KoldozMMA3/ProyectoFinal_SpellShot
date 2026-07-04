using UnityEngine;

namespace SpellShot.Gameplay
{
    public class DangerZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            TargetWord word = collision.GetComponent<TargetWord>();
            
            if (word != null)
            {
                // 1. ¡PRIMERO LA VOLVEMOS FANTASMA!: Se vuelve invisible e inmune ipso facto
                word.MarkAsCrossed();

                // 2. Luego procesamos las reglas de juego de forma segura en el mismo frame
                WaveManager waveInstance = Object.FindFirstObjectByType<WaveManager>();
                if (waveInstance != null)
                {
                    if (word.GetWordData() == waveInstance.CurrentTargetWord)
                    {
                        Debug.Log($"¡Objetivo perdido! Se escapó: {word.GetWordData().wordInEnglish}");
                        if (GameManager.Instance != null)
                        {
                            GameManager.Instance.TakeDamage(); // Quita vida legítimamente
                        }
                        
                        // Actualiza el HUD con una nueva palabra de inmediato
                        waveInstance.SelectNextTargetWord();
                    }
                }

                // 3. La eliminamos por completo de la escena
                Destroy(word.gameObject);
            }
        }
    }
}