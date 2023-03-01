using System.Collections.Generic;
using UnityEngine;

namespace Snake3
{
    public class FoodController : MonoBehaviour
    {
        [SerializeField] private Transform _foodTf;
        [SerializeField] private SnakeSegmentItem _snakeHead;

        private int _numEmptyFields;

        private void Start()
        {
            var numSegments
                = FindObjectsOfType<SnakeHeadItem>().Length
                + FindObjectsOfType<SnakeSegmentItem>().Length;
            _numEmptyFields = 8 * 8 * 6 - numSegments + 1;  // +1 so that we can call SpawnFood() immediately (which decrements this variable).
            SpawnFood();
        }

        public void OnFoodEaten()
        {
            SpawnFood();
        }

        private void SpawnFood()
        {
            // If null, the player has won
            _foodTf.position = GetRandomEmptyPosition().Value; // Todo: Exception
            _numEmptyFields--;
            // Todo: Maybe we should decrement this in reaction to creating one more body segment.
        }

        private Vector3Int? GetRandomEmptyPosition()
        {
            var randomIndex = Random.Range(0, _numEmptyFields);
            var i = 0;
            foreach (var position in IterateWorldPositions())
            {
                if (!IsPositionEmpty(position)) continue;
                if (i == randomIndex)
                {
                    return position;
                }
                i++;
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
