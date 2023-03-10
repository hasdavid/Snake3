using System.Collections.Generic;
using UnityEngine;

namespace Snake3
{
    public class FoodController : MonoBehaviour
    {
        [SerializeField] private Transform _foodTf;
        [SerializeField] private SnakeSegmentItem _snakeHead;

        private int _numEmptyFields;
        private EventManager _eventManager;

        private void Awake()
        {
            _eventManager = FindObjectOfType<EventManager>();
        }

        private void Start()
        {
            var numSegments
                = FindObjectsOfType<SnakeHeadItem>().Length
                + FindObjectsOfType<SnakeSegmentItem>().Length;
            _numEmptyFields = 8 * 8 * 6 - numSegments + 1;  // +1 so that we can call SpawnFood() immediately (which decrements this variable).
            SpawnFood();
        }

        public void OnSimulationFoodEaten()
        {
            SpawnFood();
        }

        /**
         * If the player manages to fill the whole world with Snake, there won't be any space left to spawn food.
         * In that case, the player has won and the game should end.
         */
        private void SpawnFood()
        {
            var newPos = GetRandomEmptyPosition();
            if (newPos.HasValue)
            {
                _foodTf.position = newPos.Value;
                _numEmptyFields--;
                // Todo: Maybe we should decrement this in reaction to creating one more body segment.
            }
            else
            {
                _eventManager.SimulationEnded.Invoke();
            }
        }

        private Vector3Int? GetRandomEmptyPosition()
        {
            var randomIndex = Random.Range(0, _numEmptyFields);
            var i = 0;
            foreach (var position in IterateWorldPositions())
            {
                if (IsPositionEmpty(position))
                {
                    if (i == randomIndex)
                    {
                        return position;
                    }

                    i++;
                }
            }

            // There are no empty positions.
            return null;
        }

        // Todo: Might be better to just spawn food and check for triggers.
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

        private static IEnumerable<Vector3Int> IterateWorldPositions()
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
