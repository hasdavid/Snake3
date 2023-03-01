using UnityEngine;

namespace Snake3
{
    public class SnakeController : MonoBehaviour
    {
        private InputManager _inputManager;
        private SnakeHeadItem _snakeHeadItem;

        private void Awake()
        {
            _inputManager = FindObjectOfType<InputManager>();
            _snakeHeadItem = FindObjectOfType<SnakeHeadItem>();
        }

        private void FixedUpdate()
        {
            var direction = _inputManager.LastInput;
            _snakeHeadItem.DoMovement(direction);
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
    }
}
