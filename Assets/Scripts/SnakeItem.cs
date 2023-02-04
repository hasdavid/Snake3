using System;
using UnityEngine;

namespace Snake3
{
    public class SnakeItem : MonoBehaviour
    {
        private Vector3Int _position;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void Start()
        {
            var startingPosition = Vector3Int.RoundToInt(transform.position);
            _position = startingPosition;
            _transform.position = startingPosition;
        }

        public void Move(Direction direction)
        {
            var movementDelta = Vector3Int.RoundToInt(transform.rotation * direction.AsVector3Int());
            var newPosition = _position + movementDelta;
            var rotation = Quaternion.identity;

            if (IsOverEdge(newPosition))
            {
                var (positionCorrection, rotationCorrection) = GetCorrectionForOverflow(direction);
                newPosition += positionCorrection;
                rotation = rotationCorrection;
            }

            ApplyMovement(newPosition, rotation);
        }

        private void ApplyMovement(Vector3Int newPosition, Quaternion rotation)
        {
            _position = newPosition;
            _transform.position = newPosition;
            _transform.localRotation *= rotation;
        }

        private bool IsOverEdge(Vector3Int newPosition)
        {
            // The decision is based on what side of the world we're currently on.
            return _position switch
            {
                { x: -4 or 5 } => newPosition.y is < -3 or > 4 || newPosition.z is < -3 or > 4,
                { y: -4 or 5 } => newPosition.x is < -3 or > 4 || newPosition.z is < -3 or > 4,
                { z: -4 or 5 } => newPosition.y is < -3 or > 4 || newPosition.x is < -3 or > 4,
                _ => throw new ArgumentException($"Snake is in invalid position: {_position}")
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
    }
}
