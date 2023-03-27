using UnityEngine;
using UnityEngine.Events;

namespace Snake3
{
    /**
     * Gathers all code events in one place.
     *
     * Code that requires events is expected to receive the GameObject with this component and call events through it.
     * This way we can easily see, what events are called where, and what listens to them.
     */
    public class EventManager : MonoBehaviour
    {
        /**
         * Called every time a new game starts.
         */
        public UnityEvent SimulationStarted;

        /**
         * Called when the game is paused.
         */
        public UnityEvent SimulationPaused;

        /**
         * Called when menu is closed and the game should resume.
         */
        public UnityEvent SimulationResumed;

        /**
         * Called when the game is over.
         */
        public UnityEvent SimulationEnded;

        /**
         * Called when the snake eats food.
         */
        public UnityEvent SimulationFoodEaten;
    }
}
