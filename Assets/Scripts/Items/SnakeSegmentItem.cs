using UnityEngine;

namespace Snake3
{
    public class SnakeSegmentItem : MonoBehaviour
    {
        [SerializeField] private SnakeSegmentItem _child;

        public SnakeSegmentItem Child => _child;

        public Vector3Int Position { get; private set; }

        private Transform _transform;
        private bool _hasChild;

        protected virtual void Awake()
        {
            _transform = transform;
            _hasChild = _child != null;
        }

        private void Start()
        {
            var startingPosition = Vector3Int.RoundToInt(_transform.position);
            Position = startingPosition;
            _transform.position = startingPosition;
        }

        protected void MoveWithChild(Vector3Int newPosition, bool createChild)
        {
            var oldPosition = Position;
            Position = newPosition;
            _transform.position = newPosition;
            if (_hasChild)
            {
                _child.MoveWithChild(oldPosition, createChild);
            }
            else if (createChild)
            {
                _child = Instantiate(gameObject).GetComponent<SnakeSegmentItem>();
                _hasChild = true;
                _child.MoveWithChild(oldPosition, false);
            }
        }

        protected void RotateTo(Quaternion rotation)
        {
            _transform.localRotation *= rotation;
        }
    }
}
