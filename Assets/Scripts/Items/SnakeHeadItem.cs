using System;
using UnityEngine;

namespace Snake3
{
    /**
     * Represents a snake head in game.
     */
    public class SnakeHeadItem : SnakeSegmentItem
    {
        // ----------------------------
        // Fields
        // ----------------------------

        [SerializeField] private EventManager _eventManager;
        [SerializeField] private Direction _heading;

        public Direction Heading => _heading;  // For private setter.

        private bool _createChild;

        // ----------------------------
        // Event Functions
        // ----------------------------

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Food"))
            {
                _eventManager.SimulationFoodEaten.Invoke();
                _createChild = true;
            }
            else if (other.CompareTag("BodySegment"))
            {
                _eventManager.SimulationEnded.Invoke();
            }
        }

        // ----------------------------
        // Methods
        // ----------------------------

        /**
         * Move the whole snake in the given direction.
         *
         * Direction is relative to the camera.
         *
         * Calculates where the head should end up and adds correction for overflowing the edge of the world cube. The
         * head receives rotation, which is not visible in game, but allows for easier handling of the overflowing
         * logic.
         *
         * Calls the MoveSnakeChain method, which recursively moves every Snake segment.
         */
        public void DoMovement(Direction direction)
        {
            Debug.Assert(direction != Direction.None);

            var currentRotation = transform.rotation;
            var movementDelta = Vector3Int.RoundToInt(currentRotation * direction.AsVector3Int());

            var newPosition = Position + movementDelta;
            var newRotation = currentRotation;

            if (IsOverEdge(newPosition))
            {
                var (positionCorrection, rotationCorrection) = GetCorrectionForOverflow(direction);
                newPosition += positionCorrection;
                newRotation *= rotationCorrection;
            }

            transform.rotation = newRotation;
            MoveSnakeChain(newPosition, _createChild);
            _createChild = false;
            _heading = direction;
        }

        /**
         * Test, whether the given position is out of bounds on the world cube.
         *
         * The decision is based on what side of the world we're currently on.
         */
        private bool IsOverEdge(Vector3Int position)
        {
            return Position switch
            {
                { x: -4 or 5 } => position.y is < -3 or > 4 || position.z is < -3 or > 4,
                { y: -4 or 5 } => position.x is < -3 or > 4 || position.z is < -3 or > 4,
                { z: -4 or 5 } => position.y is < -3 or > 4 || position.x is < -3 or > 4,
                _ => throw new ArgumentException($"Snake is in invalid position: {Position}")
            };
        }

        /**
         * Calculate position and rotation correction for world edge overflow in given direction.
         *
         * Thanks to Unity's built-in rotation system, we can skip complicated if-else chains here, and just calculate
         * the correction relative to current rotation.
         */
        private (Vector3Int, Quaternion) GetCorrectionForOverflow(Direction dir)
        {
            var positionCorrection = Vector3Int.RoundToInt(transform.rotation * new Vector3(0, 0, 1));
            var rotationCorrection = dir switch
            {
                Direction.Up => Quaternion.Euler(90, 0, 0),
                Direction.Down => Quaternion.Euler(-90, 0, 0),
                Direction.Left => Quaternion.Euler(0, 90, 0),
                Direction.Right => Quaternion.Euler(0, -90, 0),
                Direction.None => throw new ArgumentException("Overflow direction is none, which not valid."),
                _ => throw new System.ComponentModel.InvalidEnumArgumentException(nameof(dir), (int)dir,
                    dir.GetType())
            };

            return (positionCorrection, rotationCorrection);
        }
    }
}
