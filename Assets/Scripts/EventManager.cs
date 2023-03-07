using UnityEngine;
using UnityEngine.Events;

namespace Snake3
{
    public class EventManager : MonoBehaviour
    {
        public UnityEvent FoodEaten;
        public UnityEvent GameOver;
        public UnityEvent GameResumed;
        public UnityEvent GamePaused;
    }
}
