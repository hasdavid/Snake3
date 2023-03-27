using UnityEngine;

namespace Snake3
{
    public class SnakeController : MonoBehaviour
    {
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private SnakeHeadItem _snakeHeadItem;
        [SerializeField] private GameObject _headAliveGo;
        [SerializeField] private GameObject _headDeadGo;
        private Direction _lastInput;

        private void Start()
        {
            enabled = false;
        }

        /**
         * Every game tick, move the Snake according to user's last input.
         */
        private void FixedUpdate()
        {
            var direction = ReadInput();

            if (!direction.HasValue)
            {
                direction = _snakeHeadItem.Heading;
            }

            _snakeHeadItem.DoMovement(direction.Value);
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
            _headAliveGo.SetActive(false);
            _headDeadGo.SetActive(true);
            enabled = false;
        }

        private Direction? ReadInput()
        {
            Direction? direction;

            // Iterate through every Direction in the queue, popping them, until we find one that's valid, or empty the list.
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
