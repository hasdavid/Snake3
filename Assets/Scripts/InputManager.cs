using System;
using UnityEngine;

namespace Snake3
{
    public class InputManager : MonoBehaviour
    {
        public Direction LastInput { get; private set; }

        private bool _wasFixedUpdate;

        private void Update()
        {
            if (_wasFixedUpdate)
            {
                _wasFixedUpdate = false;
                LastInput = Direction.None;
            }
            CaptureInput();
        }

        private void FixedUpdate()
        {
            _wasFixedUpdate = true;
        }

        private void CaptureInput()
        {
            var horizontal = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
            var vertical = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));

            // Prefer horizontal input.
            LastInput = (horizontal, vertical) switch
            {
                (-1, 0) or (-1, -1) or (-1, 1) => Direction.Left,
                (1, 0) or (1, -1) or (1, 1) => Direction.Right,
                (0, 1) => Direction.Up,
                (0, -1) => Direction.Down,
                (0, 0) => LastInput,
                _ => throw new ArgumentException($"Invalid input: {(horizontal, vertical)}")
            };
        }
    }
}
