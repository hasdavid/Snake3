using System.Collections.Generic;
using UnityEngine;

namespace Snake3
{
    /**
     * Gathers controls from the user.
     *
     * Only takes care of the snake controls - which direction it should move. This class doesn't handle input
     * regarding pausing and unpausing.
     */
    public class InputManager : MonoBehaviour
    {
        // ----------------------------
        // Fields
        // ----------------------------

        private readonly Queue<Direction> _inputQueue = new();

        private bool _rightRegistered;
        private bool _leftRegistered;
        private bool _upRegistered;
        private bool _downRegistered;

        // ----------------------------
        // Event Functions
        // ----------------------------

        private void Update()
        {
            RegisterButtonsDown();
        }

        // ----------------------------
        // Methods
        // ----------------------------

        /**
         * Pop an item from the front of the input queue.
         */
        public Direction PopQueue()
        {
            return _inputQueue.Dequeue();
        }

        /**
         * Clear the input queue.
         */
        public void ClearQueue()
        {
            _inputQueue.Clear();
        }

        /**
         * Returns true, if the input queue is empty, or false otherwise.
         */
        public bool IsQueueEmpty()
        {
            return _inputQueue.Count <= 0;
        }

        /**
         * Register which buttons were pressed.
         *
         * This variant adds only buttons, that were just pressed, into the input queue. Not the buttons, which are
         * held. Results in slightly different feel than RegisterButtonsAll().
         */
        private void RegisterButtonsDown()
        {
            // Determine what buttons are currently pressed
            var right = Mathf.RoundToInt(Input.GetAxisRaw("Right"));
            var left  = Mathf.RoundToInt(Input.GetAxisRaw("Left"));
            var up    = Mathf.RoundToInt(Input.GetAxisRaw("Up"));
            var down  = Mathf.RoundToInt(Input.GetAxisRaw("Down"));

            // Enqueue buttons, that were pressed just now
            if (right > 0 && !_rightRegistered)
            {
                _inputQueue.Enqueue(Direction.Right);
                _rightRegistered = true;
            }

            if (left > 0 && !_leftRegistered)
            {
                _inputQueue.Enqueue(Direction.Left);
                _leftRegistered = true;
            }

            if (up > 0 && !_upRegistered)
            {
                _inputQueue.Enqueue(Direction.Up);
                _upRegistered = true;
            }

            if (down > 0 && !_downRegistered)
            {
                _inputQueue.Enqueue(Direction.Down);
                _downRegistered = true;
            }

            // Remember, which buttons were released
            if (right <= 0) _rightRegistered = false;
            if (left  <= 0) _leftRegistered  = false;
            if (up    <= 0) _upRegistered    = false;
            if (down  <= 0) _downRegistered  = false;
        }

        /**
         * Register which buttons were pressed.
         *
         * This variant adds all currently held buttons into the input queue. Results in slightly different feel than
         * RegisterButtonsDown().
         *
         * Currently unused, but kept here for easy switching of the two, should the issue of snake controls ever be
         * reopened.
         */
        private void RegisterButtonsAll()
        {
            var right = Mathf.RoundToInt(Input.GetAxisRaw("Right"));
            var left = Mathf.RoundToInt(Input.GetAxisRaw("Left"));
            var up = Mathf.RoundToInt(Input.GetAxisRaw("Up"));
            var down = Mathf.RoundToInt(Input.GetAxisRaw("Down"));

            if (right > 0) _inputQueue.Enqueue(Direction.Right);
            if (left > 0) _inputQueue.Enqueue(Direction.Left);
            if (up > 0) _inputQueue.Enqueue(Direction.Up);
            if (down > 0) _inputQueue.Enqueue(Direction.Down);
        }
    }
}
