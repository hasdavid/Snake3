using UnityEngine;

namespace Snake3
{
    /**
     * Enumeration containing four directions and a special None value.
     */
    public enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public static class DirectionExtensions
    {
        /**
         * Return a Vector3Int value corresponding to a given Direction value.
         */
        public static Vector3Int AsVector3Int(this Direction direction)
        {
            return direction switch
            {
                Direction.None => Vector3Int.zero,
                Direction.Up => Vector3Int.up,
                Direction.Down => Vector3Int.down,
                Direction.Left => Vector3Int.left,
                Direction.Right => Vector3Int.right,
                _ => throw new System.ComponentModel.InvalidEnumArgumentException(
                    nameof(direction), (int)direction, direction.GetType())
            };
        }

        /**
         * Return the opposite Direction.
         */
        public static Direction Opposite(this Direction direction)
        {
            return direction switch
            {
                Direction.None => Direction.None,
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                _ => throw new System.ComponentModel.InvalidEnumArgumentException(
                    nameof(direction), (int)direction, direction.GetType())
            };
        }
    }
}
