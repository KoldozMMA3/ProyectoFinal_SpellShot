using UnityEngine;

namespace SpellShot.Data
{
    [CreateAssetMenu(fileName = "NewWordData", menuName = "SpellShot/Word Data")]
    public class WordData : ScriptableObject
    {
        [Header("Configuración Lingüística")]
        [Tooltip("La palabra o letra en inglés que se mostrará en las pantallas u objetivos.")]
        public string wordInEnglish;
        
        [Tooltip("La traducción correcta al español para mostrar cuando el jugador comete un error.")]
        public string translationToSpanish;

        [Header("Estímulo Visual (Requerimiento 4)")]
        [Tooltip("Imagen opcional asociada a la palabra (Nivel A1-A2).")]
        public Sprite visualStimulus;

        [Tooltip("Definición simple o pista corta en inglés en caso de no usar imagen.")]
        [TextArea(2, 4)]
        public string simpleDefinition;

        [Header("Lógica de Juego (Requerimiento 2)")]
        [Tooltip("Si está marcado, esta palabra es la respuesta correcta para el estímulo visual/definición de la oleada.")]
        public bool isCorrectTarget;

        [Header("Progresión de Niveles (Requerimiento 8)")]
        [Tooltip("Nivel al que pertenece la palabra (1: Básico/Objetos, 2: Intermedio/Profesiones, 3: Avanzado/Números).")]
        [Range(1, 3)]
        public int levelCategory = 1;
    }
}