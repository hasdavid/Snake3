using UnityEngine;

namespace Snake3
{
    /**
     * Controls the behavior of the Snake.
     *
     * We use Unity's FixedUpdate() to progress the game by moving Snake.
     */
    public class SnakeController : MonoBehaviour
    {
        // ----------------------------
        // Fields
        // ----------------------------

        [SerializeField] private InputManager _inputManager;
        [SerializeField] private SnakeHeadItem _snakeHeadItem;
        [SerializeField] private GameObject _headAliveGo;
        [SerializeField] private GameObject _headDeadGo;

        private Direction _lastInput;

        // ----------------------------
        // Event Functions
        // ----------------------------

        private void Start()
        {
            enabled = false;
        }

        private void FixedUpdate()
        {
            var direction = ReadInput() ?? _snakeHeadItem.Heading;
            _snakeHeadItem.DoMovement(direction);
        }

        public void OnSimulationStarted()
        {
            enabled = true;
        }

        public void OnSimulationPaused()
        {
            enabled = false;
        }

        public void OnSimulationResumed()
        {
            enabled = true;
        }

        public void OnSimulationEnded()
        {
            // Replace the head with its dead model.
            _headAliveGo.SetActive(false);
            _headDeadGo.SetActive(true);
            enabled = false;
        }

        // ----------------------------
        // Methods
        // ----------------------------

        /**
         * Tries to get a Direction from the input queue of the InputManager.
         *
         * Iterates through the queue, popping the directions, until it finds one, that's valid, or empties the list.
         *
         * Clears the queue afterwards.
         *
         * Returns null, if no valid direction was found.
         */
        private Direction? ReadInput()
        {
            Direction? direction;

            while (true)
            {
                // If we found no usable input, return null.
                if (_inputManager.IsQueueEmpty())
                {
                    direction = null;
                    break;
                }

                var item = _inputManager.PopQueue();

                // If we found a usable input, return it.
                if (item != _snakeHeadItem.Heading && item != _snakeHeadItem.Heading.Opposite())
                {
                    direction = item;
                    break;
                }
            }

            // Forget any extra input
            _inputManager.ClearQueue();

            return direction;
        }
    }
}
