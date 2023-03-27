using System.Collections.Generic;
using UnityEngine;

namespace Snake3
{
    /**
     * Controls spawning of food.
     */
    public class FoodController : MonoBehaviour
    {
        // ----------------------------
        // Fields
        // ----------------------------

        [SerializeField] private Transform _foodTf;
        [SerializeField] private SnakeSegmentItem _snakeHead;
        [SerializeField] private EventManager _eventManager;

        private int _numEmptyFields;

        // ----------------------------
        // Event Functions
        // ----------------------------

        private void Start()
        {
            var numSegments = _snakeHead.transform.parent.childCount;
            // We increment by one so that we can immediately decrement in SpawnFood().
            _numEmptyFields = 8 * 8 * 6 - numSegments + 1;

            SpawnFood();
        }

        public void OnSimulationFoodEaten()
        {
            SpawnFood();
        }

        // ----------------------------
        // Methods
        // ----------------------------

        /**
         * Spawn new food.
         *
         * Doesn't actually instantiate anything, just moves the same GameObject.
         *
         * If the player manages to fill the whole world with Snake, there won't be any space left to spawn food.
         * In that case, the player has won and the game will end.
         */
        private void SpawnFood()
        {
            var newPos = GetRandomEmptyPosition();
            if (newPos.HasValue)
            {
                _foodTf.position = newPos.Value;
                _numEmptyFields--;
            }
            else
            {
                // No free space left, the player has won.
                _eventManager.SimulationEnded.Invoke();
            }
        }

        /**
         * Find and return a randomized empty position in the game world.
         *
         * Generates a random number N in the range of number of currently unoccupied spaces.
         * Then tries to find N-th empty space in the world, always looping in the same order.
         * Might not be the most efficient, but should cover the world uniformly.
         *
         * Returns coordinates of the empty position, or null, if there is none.
         */
        private Vector3Int? GetRandomEmptyPosition()
        {
            if (_numEmptyFields <= 0) return null;

            var targetIndex = Random.Range(0, _numEmptyFields);
            var i = 0;
            foreach (var position in GetWorldPositionEnumerator())
            {
                if (!IsPositionEmpty(position)) continue;

                if (i == targetIndex)
                {
                    return position;
                }

                i++;
            }

            // There are no empty positions.
            return null;
        }

        /**
         * Check if given position is empty.
         *
         * Iterates through every snake segment and compares their position against the given one.
         *
         * Might not be the most efficient way to achieve that.
         */
        private bool IsPositionEmpty(Vector3Int position)
        {
            var segment = _snakeHead;

            while (segment is not null)
            {
                if (position == segment.Position)
                {
                    return false;
                }
                segment = segment.Child;
            }

            return true;
        }

        /**
         * Return an enumerator which traverses the world positions in the same order every time.
         */
        private static IEnumerable<Vector3Int> GetWorldPositionEnumerator()
        {
            for (var a = -3; a <= 4; a++)
            {
                for (var b = -3; b <= 4; b++)
                {
                    yield return new Vector3Int(-4, a, b);
                    yield return new Vector3Int(+5, a, b);
                    yield return new Vector3Int(a, -4, b);
                    yield return new Vector3Int(a, +5, b);
                    yield return new Vector3Int(a, b, -4);
                    yield return new Vector3Int(a, b, +5);
                }
            }
        }
    }
}
