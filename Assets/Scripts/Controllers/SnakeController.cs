using System;
using UnityEngine;

namespace Snake3
{
    public class SnakeController : MonoBehaviour
    {
        [SerializeField] private SnakeHeadItem _snakeHeadItem;
        [SerializeField] private GameObject _headAliveGo;
        [SerializeField] private GameObject _headDeadGo;
        private Direction _latestInput;

        private void Start()
        {
            enabled = false;
        }

        /**
         * Every frame, take user's input and store it, overwriting any previous input.
         */
        private void Update()
        {
            var newDirection = GetDirectionFromInput();
            if (newDirection is not Direction.None)
            {
                _latestInput = newDirection;
            }
        }

        /**
         * Every game tick, move the Snake according to user's last input.
         */
        private void FixedUpdate()
        {
            _snakeHeadItem.DoMovement(_latestInput);
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

        private static Direction GetDirectionFromInput()
        {
            var horizontal = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
            var vertical = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));

            // Prefer horizontal input.
            return (horizontal, vertical) switch
            {
                (-1, 0) or (-1, -1) or (-1, 1) => Direction.Left,
                (1, 0) or (1, -1) or (1, 1) => Direction.Right,
                (0, 1) => Direction.Up,
                (0, -1) => Direction.Down,
                (0, 0) => Direction.None,
                _ => throw new ArgumentException($"Invalid input: {(horizontal, vertical)}")
            };
        }
    }
}
