using System;
using UnityEngine;

namespace Snake3
{
    public class SnakeHeadItem : SnakeSegmentItem
    {
        [SerializeField] private Direction _lastDirection;

        private bool _createChild;
        private EventManager _eventManager;

        protected override void Awake()
        {
            base.Awake();
            _eventManager = FindObjectOfType<EventManager>();
        }

        public void DoMovement(Direction direction)
        {
            // Todo: This may be better in SnakeController
            if (direction == Direction.None || direction == _lastDirection.Opposite())
            {
                direction = _lastDirection;
            }

            var movementDelta = Vector3Int.RoundToInt(transform.rotation * direction.AsVector3Int());
            var newPosition = Position + movementDelta;
            var rotation = Quaternion.identity;

            if (IsOverEdge(newPosition))
            {
                var (positionCorrection, rotationCorrection) = GetCorrectionForOverflow(direction);
                newPosition += positionCorrection;
                rotation = rotationCorrection;
            }

            RotateTo(rotation);
            MoveWithChild(newPosition, _createChild);
            _createChild = false;
            _lastDirection = direction;
        }

        private bool IsOverEdge(Vector3Int newPosition)
        {
            // The decision is based on what side of the world we're currently on.
            return Position switch
            {
                { x: -4 or 5 } => newPosition.y is < -3 or > 4 || newPosition.z is < -3 or > 4,
                { y: -4 or 5 } => newPosition.x is < -3 or > 4 || newPosition.z is < -3 or > 4,
                { z: -4 or 5 } => newPosition.y is < -3 or > 4 || newPosition.x is < -3 or > 4,
                _ => throw new ArgumentException($"Snake is in invalid position: {Position}")
            };
        }

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
                _ => throw new System.ComponentModel.InvalidEnumArgumentException(nameof(dir), (int)dir, dir.GetType())
            };
            return (positionCorrection, rotationCorrection);
        }

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
    }
}
