using UnityEngine;

namespace Snake3
{
    public class SnakeController : MonoBehaviour
    {
        private InputManager _inputManager;
        private SnakeItem _snakeItem;

        private void Awake()
        {
            _inputManager = FindObjectOfType<InputManager>();
            _snakeItem = FindObjectOfType<SnakeItem>();
        }

        private void FixedUpdate()
        {
            var direction = _inputManager.LastInput;
            _snakeItem.Move(direction);
        }
    }
}
