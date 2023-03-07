using System;
using UnityEngine;

namespace Snake3
{
    public class SnakeController : MonoBehaviour
    {
        private SnakeHeadItem _snakeHeadItem;
        private Direction _latestInput;

        private void Awake()
        {
            _snakeHeadItem = FindObjectOfType<SnakeHeadItem>();
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

        public void OnGamePaused()
        {
            enabled = false;
        }

        public void OnGameResumed()
        {
            enabled = true;
        }

        public void OnGameOver()
        {
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
