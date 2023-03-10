using UnityEngine;
using UnityEngine.Events;

namespace Snake3
{
    public class EventManager : MonoBehaviour
    {
        public UnityEvent SimulationStarted;
        public UnityEvent SimulationPaused;
        public UnityEvent SimulationResumed;
        public UnityEvent SimulationEnded;
        public UnityEvent SimulationFoodEaten;
    }
}
